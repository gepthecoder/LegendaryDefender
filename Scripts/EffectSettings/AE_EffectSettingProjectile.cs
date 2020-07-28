using UnityEngine;
using System.Collections;

public class AE_EffectSettingProjectile : MonoBehaviour
{
    public float Mass = 1;
    public float Speed = 10;
    public float AirDrag = 0.1f;
    public bool UseGravity = true;

    //void Awake()
    //{
    //    prevMass = Mass;
    //    prevSpeed = Speed;
    //    prevRandomSpeedOffset = RandomSpeedOffset;
    //    prevAirDrag = AirDrag;
    //    prevUseGravity = UseGravity;

    //    var transformMotion = GetComponentInChildren<AE_PhysicsMotion>(true);
    //    if (transformMotion != null)
    //    {
    //        startSpeed = transformMotion.Speed;
    //    }
    //}

    void OnEnable()
    {
        var physicsMotion = GetComponentInChildren<AE_PhysicsMotion>(true);
        if (physicsMotion != null)
        {
            //transformMotion.Distance = FlyDistanceForProjectiles;
            physicsMotion.Mass = Mass;
            physicsMotion.Speed = Speed;
            physicsMotion.AirDrag = AirDrag;
            physicsMotion.UseGravity = UseGravity;
        }
    }
}
