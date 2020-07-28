using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_DistanceOffset : MonoBehaviour {

    public AnimationCurve PositionX = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);
    public Vector2 DistanceScaleX = new Vector2(1, 1);
    public AnimationCurve PositionY = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);
    public Vector2 DistanceScaleY = new Vector2(1, 1);
    public AnimationCurve PositionZ = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);
    public Vector2 DistanceScaleZ = new Vector2(1, 1);

    public Transform Object;

    float currentDistance;
    Vector3 prevPos;

    void OnEnable ()
    {

    }

	void Update ()
	{
        currentDistance += Mathf.Abs((transform.position - prevPos).magnitude);
	    prevPos = transform.position;


        //var distanceX = Mathf.Abs(transform.position.x - startPoint.x) / DistanceScaleX.x;
        var offsetX = (PositionX.Evaluate(currentDistance % 1) * 2 - 1) * DistanceScaleX.y;

        //var distanceY = Mathf.Abs(transform.position.y - startPoint.y) / DistanceScaleY.x;
        var offsetY = (PositionY.Evaluate(currentDistance % 1) * 2 - 1) * DistanceScaleY.y;

        //var distanceZ = Mathf.Abs(transform.position.z - startPoint.z) / DistanceScaleZ.x;
        var offsetZ = (PositionZ.Evaluate(currentDistance % 1) * 2 - 1) * DistanceScaleZ.y;
        Object.transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
    }
}
