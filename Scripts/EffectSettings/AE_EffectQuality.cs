using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_EffectQuality : MonoBehaviour {

    [Range(1, 15000)]
    public int MaxParticles = 15000;
    [Range(2000, 20000)]
    public int MaxParticlesForAllEffects = 20000;

    Dictionary<ParticleSystem, int> starMaxParticles = new Dictionary<ParticleSystem, int>();
    AE_SharedParticlesQualitySetting sharedSettings;
    int currentMaxParticles;
    //ParticleSystem[] particles;


    void Awake()
    {
        sharedSettings = AE_SharedParticlesQualitySetting.getInstance();

        var particles = GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in particles)
        {
            starMaxParticles.Add(ps, ps.main.maxParticles);
            currentMaxParticles += ps.main.maxParticles;
        }

    }

    private void OnEnable()
    {
        int currentParticles = 0;
        if (currentMaxParticles > MaxParticles)
        {
            // currentMaxParticles = 0;
            foreach (var ps in starMaxParticles)
            {
                var main = ps.Key.main;
                var maxParticles = (ps.Value * 1.0f / MaxParticlesForAllEffects * 1.0f) * MaxParticles;
                if (ps.Value > MaxParticles) maxParticles *= Mathf.Clamp01(maxParticles * 1.0f / currentMaxParticles * 1.0f);
                main.maxParticles = (int)maxParticles;
                currentParticles += main.maxParticles;
            }
        }
        else currentParticles = currentMaxParticles;
        sharedSettings.OptimizeParticlesCount(MaxParticlesForAllEffects, currentParticles);

        foreach (var ps in starMaxParticles)
        {
            sharedSettings.CurentParticleSystems.Add(ps.Key, ps.Value);
        }
    }


    private void OnDisable()
    {
        foreach (var ps in starMaxParticles)
        {
            sharedSettings.CurentParticleSystems.Remove(ps.Key);
        }
        sharedSettings.OptimizeParticlesCount(MaxParticlesForAllEffects, 0);
    }

}
