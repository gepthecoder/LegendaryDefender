using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Raycast destroyer (GabroMedia)")]
public class Destruct_Camera : MonoBehaviour {

	//GABROMEDIA@GMAIL.COM

	//This class finds camera, casts ray, triggers gunshot on mouseclick, identifies GabroMedia destructible asset by Destruct class existence, reads out two variables
	//from it (break sound effect and fractured prefab), destroys the instance, instantiates fractured model on same position and removes debris from the scene, taking
	//the user set preferences in consideration.

	Camera cam;
	bool inCycle;
	GameObject smoke = null;

	public AudioClip[] shotSound;
	[Range(10, 60)]
	public float removeDebrisDelay;
	[Range(5,40)]
	public float impactForce;
	[Range(0, 1)]
	public float gunShotFrequency;

	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	void Update () {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
			if (Input.GetMouseButton(0) && !inCycle) {
				StartCoroutine(delay(gunShotFrequency));
				AudioSource.PlayClipAtPoint(shotSound[Random.Range(0, shotSound.Length)], transform.position);
				if (hit.transform.GetComponent<Destruct> ()) {
					Destruct d = hit.transform.GetComponent<Destruct> ();
					BreakObject (d, ray.direction, hit.point);
				}
			}
		}
	}

	private void BreakObject(Destruct instance, Vector3 rayDirection, Vector3 hitPoint) {
		GameObject fractured = instance.fracturedPrefab;
		GameObject smokeEffect = instance.smokeParticle;
		AudioClip breakSound = instance.shatter;
		Vector3 pos = instance.transform.position;
		Quaternion rot = instance.transform.rotation;
		Destroy (instance.gameObject);
		if (smokeEffect) {
			Vector3 heightFix = new Vector3(pos.x, pos.y +1f, pos.z);
			smoke = (GameObject) Instantiate (smokeEffect, heightFix, rot);
		}
		AudioSource.PlayClipAtPoint (breakSound, pos);
		GameObject go = (GameObject)Instantiate (fractured, pos, rot);
		go.name = "FracturedClone";
		foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>()) {
			rb.AddForceAtPosition (rayDirection * impactForce, hitPoint, ForceMode.Impulse);
		}
		StartCoroutine (removeDebris (go, removeDebrisDelay));
		if (smoke) {
			Destroy(smoke, 2f);
		}
	}

	IEnumerator delay(float secs) {
		inCycle = true;
		yield return new WaitForSeconds(secs);
		inCycle = false;
	}

	IEnumerator removeDebris(GameObject go, float seconds) {
		yield return new WaitForSeconds (seconds);
		Destroy (go);
	}
}
