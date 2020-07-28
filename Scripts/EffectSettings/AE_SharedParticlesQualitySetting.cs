
using System.Collections.Generic;
using UnityEngine;

public class AE_SharedParticlesQualitySetting {

    public Dictionary<ParticleSystem, int> CurentParticleSystems = new Dictionary<ParticleSystem, int>();

    public void OptimizeParticlesCount(int maxParticlesBudget, int currentParticles)
    {
        if (CurentParticleSystems.Count == 0) return;

        var currentBudget = maxParticlesBudget - currentParticles;
        var currentMaxAllParticles = GetMaxAllParticles();
        

        foreach (var ps in CurentParticleSystems)
        {
            var main = ps.Key.main;
            float maxParticles;
            if (currentMaxAllParticles < currentBudget)
                maxParticles = ps.Value;
            else
                maxParticles = (ps.Value * 1.0f / currentMaxAllParticles * 1.0f) * currentBudget;

            main.maxParticles = (int)maxParticles;
        }
        
    }

    int GetMaxAllParticles()
    {
        int currentMaxParticles = 0;
        foreach (var ps in CurentParticleSystems)
        {
            currentMaxParticles += ps.Value;
        }
        return currentMaxParticles;
    }

    private static AE_SharedParticlesQualitySetting instance;
  
    public static AE_SharedParticlesQualitySetting getInstance()
    {
        if (instance == null)
            instance = new AE_SharedParticlesQualitySetting();
        return instance;
    }
}