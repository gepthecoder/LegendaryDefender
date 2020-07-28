using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_SimpleDecal : MonoBehaviour
{
    public float Offset = 0.05f;
    private Transform t;
    bool canUpdate;
    public float Distance = 5;

    private MeshRenderer meshRend;
    // Use this for initialization
    void Awake ()
	{
	    t = transform;
    }

    private void OnEnable()
    {
        canUpdate = true;
        meshRend = GetComponent<MeshRenderer>();
        if(meshRend!=null) meshRend.enabled = true;
        
        //InvokeRepeating("UpdatePosition", 0, 0.2f);
    }

    private RaycastHit hit;

    private float timer;
    // Update is called once per frame
    void Update()
	{
        if (!canUpdate) return;

	    if (Physics.Raycast(t.parent.position - t.forward / 2, t.forward, out hit, Distance))
	    {
            var skinnedMesh = hit.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();

            if (skinnedMesh != null)
            {
                if (meshRend != null) meshRend.enabled = false;
                return;
            }
            transform.position = hit.point - transform.forward * Offset;
            transform.rotation = Quaternion.LookRotation(-hit.normal);
	    }
	}
    

    private void OnDisable()
    {
        canUpdate = false; //can't use cancelInvoke, because it's caused crash
    }
}
