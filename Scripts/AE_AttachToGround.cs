using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_AttachToGround : MonoBehaviour {
    public float Offset = 0.05f;

    bool needUpdate;
    // Use this for initialization
    void OnEnable () {
        needUpdate = true;
       
    }
	
	// Update is called once per frame
	void Update () {
		if(needUpdate)
        {
            needUpdate = false;
            var t = transform;
            RaycastHit hit;
            float minPos = t.position.y;
            if (Physics.Raycast(t.position + Vector3.up / 2, Vector3.down, out hit))
            {
                minPos = Mathf.Min(minPos, hit.point.y);
            }
            if (Physics.Raycast(t.position + Vector3.up / 2 + new Vector3(1, 0, 0), Vector3.down, out hit))
            {
                minPos = Mathf.Min(minPos, hit.point.y);
            }

            if (Physics.Raycast(t.position + Vector3.up / 2 + new Vector3(-1, 0, 0), Vector3.down, out hit))
            {
                minPos = Mathf.Min(minPos, hit.point.y);
            }

            if (Mathf.Abs(t.position.y - minPos) > 0.001f)
            {
                transform.position = new Vector3(t.position.x, minPos, t.position.z);
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
               
            }
        }
	}
}