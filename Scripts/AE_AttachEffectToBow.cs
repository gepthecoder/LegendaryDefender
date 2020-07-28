using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_AttachEffectToBow : MonoBehaviour
{
    public GameObject Bow;
    const string BowName = "Bow";
    bool isInitialized;
    Transform child;

    void OnEnable()
    {
        isInitialized = false;
        child = transform.GetChild(0);
        child.localPosition = Vector3.zero;
        child.localRotation = new Quaternion();
    }

    void LateUpdate()
    {
        if (Bow == null) return;

        child.position = Bow.transform.position;
        child.rotation = Bow.transform.rotation;
    }

    void Update()
	{
        if (Bow == null || isInitialized) return;

        isInitialized = true;

        var bowMesh = Bow.GetComponent<MeshRenderer>();
	    if (bowMesh == null) return;

	    var particles = GetComponentsInChildren<ParticleSystem>();
	    foreach (var ps in particles)
	    {
	        if (ps.name.Contains(BowName))
	        {
	            var shape = ps.shape;
	            shape.useMeshColors = false;
	            shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
	            shape.shapeType = ParticleSystemShapeType.MeshRenderer;
	            shape.meshRenderer = bowMesh;
	        }
	    }

        var bowMeshFiltres = GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in bowMeshFiltres)
        {
            if (meshFilter.name.Contains(BowName))
            {
                meshFilter.sharedMesh = Bow.GetComponent<MeshFilter>().sharedMesh;
                meshFilter.transform.rotation = Bow.transform.rotation;
                meshFilter.transform.position = Bow.transform.position;
            }
        }
	}


}
