using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class AE_BowString : MonoBehaviour
{
    public float StringTint = 0.01f;
    public Transform Point1;
    public Transform Point2;
    public Transform HandBone;
    public bool InHand;

    LineRenderer lineRenderer;
    Vector3 prevHandPos;
    const float tensionTime = 0.03f;
    float currentTensionTime;
	// Use this for initialization
	void OnEnable ()
	{
	    lineRenderer = GetComponent<LineRenderer>();

         lineRenderer.widthMultiplier = StringTint;

        if (Point1 == null || Point2 == null || HandBone == null) return;

        lineRenderer.positionCount = 3;
	    prevHandPos = (Point1.position + Point2.position)/2;
        lineRenderer.SetPosition(0, Point1.position);
        lineRenderer.SetPosition(1, prevHandPos);
        lineRenderer.SetPosition(2, Point2.position);
    }

	// Update is called once per frame
	void Update ()
	{
	    if (Point1 == null || Point2 == null || HandBone == null) return;

        lineRenderer.widthMultiplier = StringTint;

        if (InHand)
	    {
	        lineRenderer.positionCount = 3;
	        lineRenderer.SetPosition(0, Point1.position);
	        lineRenderer.SetPosition(1, HandBone.position);
	        lineRenderer.SetPosition(2, Point2.position);
	        currentTensionTime = 0;
	        prevHandPos = HandBone.position;

	    }
	    else
	    {
	        currentTensionTime += Time.deltaTime;
            lineRenderer.positionCount = 3;
            var defaultPos = (Point1.position + Point2.position) / 2;
            lineRenderer.SetPosition(0, Point1.position);
            lineRenderer.SetPosition(1, Vector3.Lerp(prevHandPos, defaultPos, Mathf.Clamp01(currentTensionTime / tensionTime)));
            lineRenderer.SetPosition(2, Point2.position);
        }
	}
}
