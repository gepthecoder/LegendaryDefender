using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_ParticlesSizeOverTime : MonoBehaviour {

    public AnimationCurve SizeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float LifeTime = 1;
    public Vector3 SizeMul = Vector3.one;
    
    public bool IsLoop;

    private bool canUpdate;
    private float startTime;
    private ParticleSystem.MainModule mainPS;

    private void Awake()
    {
        var ps = GetComponent<ParticleSystem>();
        mainPS = ps.main;
    }

    private void OnEnable()
    {
        startTime = Time.time + mainPS.startDelay.constant;
        canUpdate = true;
        var mul = SizeCurve.Evaluate(0);
        if (mainPS.startSize3D)
        {
            mainPS.startSizeXMultiplier = mul * SizeMul.x;
            mainPS.startSizeYMultiplier = mul * SizeMul.y;
            mainPS.startSizeZMultiplier = mul * SizeMul.z;
        }
        else mainPS.startSizeMultiplier = mul * SizeMul.x;
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate)
        {
            var mul = SizeCurve.Evaluate(time / LifeTime);
            if (mainPS.startSize3D)
            {
                mainPS.startSizeXMultiplier = mul * SizeMul.x;
                mainPS.startSizeYMultiplier = mul * SizeMul.y;
                mainPS.startSizeZMultiplier = mul * SizeMul.z;
            }
            else mainPS.startSizeMultiplier = mul * SizeMul.x;
        }
        if (time >= LifeTime)
        {
            if (IsLoop) startTime = Time.time;
            else canUpdate = false;
        }
    }
}
