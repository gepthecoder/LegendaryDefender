using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_AttachEffectToArrow : MonoBehaviour
{
    public GameObject Arrow;

    const string ArrowName = "Arrow";
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
        if (Arrow == null) return;

        child.position = Arrow.transform.position;
        child.rotation = Arrow.transform.rotation;
    }


    void Update()
    {
        if (Arrow == null || isInitialized) return;

        isInitialized = true;

        var arrowMesh = Arrow.GetComponent<MeshRenderer>();
        if (arrowMesh == null) return;

        var arrowScale = arrowMesh.transform.lossyScale;

        var particleScale = Mathf.Min((Mathf.Min(arrowScale.x, arrowScale.y)), arrowScale.z);

        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            if (ps.name.Contains(ArrowName))
            {
                ps.gameObject.SetActive(false);
                var main = ps.main;
                main.startSizeMultiplier /= particleScale;

                var shape = ps.shape;
                shape.useMeshColors = false;
                shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
                shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                shape.meshRenderer = arrowMesh;
                ps.gameObject.SetActive(true);
            }
        }

        var arrowMeshMeshFiltres = GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in arrowMeshMeshFiltres)
        {
            if (meshFilter.name.Contains(ArrowName))
            {
                meshFilter.sharedMesh = Arrow.GetComponent<MeshFilter>().sharedMesh;
                meshFilter.transform.rotation = Arrow.transform.rotation;
                meshFilter.transform.position = Arrow.transform.position;
                meshFilter.transform.localScale = arrowScale;
            }
        }
    }
}

