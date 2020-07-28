using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_FollowMotion : MonoBehaviour {

    public AnimationCurve SlowCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float FollowTime = 1;
    public Transform Target;
    public float currentVel = 0;
    private bool canUpdate;
    private float startTime;
    private Vector3 startWorldPos;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;
        transform.localPosition = Vector3.zero;
        startWorldPos = transform.position;
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate)
        {
            var eval = SlowCurve.Evaluate(time / FollowTime);
            transform.position = Vector3.Lerp(startWorldPos, Target.position, eval);
        }
        if (time >= FollowTime) canUpdate = false;
    }
}
