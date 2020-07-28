using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_WorldSpaceRotationAtStart : MonoBehaviour {

    Vector3 startRotation;
	void Awake () {
        startRotation = transform.localRotation.eulerAngles;
	}

	void OnEnable () {
        transform.rotation = Quaternion.Euler(startRotation);
	}
}
