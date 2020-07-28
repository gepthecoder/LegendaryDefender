using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_SetMeshToEffect : MonoBehaviour {

    public EffectMeshType MeshType = EffectMeshType.Bow;
    public GameObject Mesh;
    public bool UseMeshPosition = true;

    bool isInitialized;
    List<ParticleSystem> particles = new List<ParticleSystem>();
    List<MeshFilter> meshes = new List<MeshFilter>();

    void InitializeEffect()
    {
        isInitialized = true;

        var meshRenderer = Mesh.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.Log("KriptoFX/ArcherEffects... Can't find a mesh with 'mesh renderer' for effect. You need set a bow/arrow mesh. Please read the readme");
            return;
        }
        var meshName = MeshType.ToString();
        var particleScale = Mesh.GetComponent<Renderer>().bounds.size.magnitude;
       // var particleScale = Mathf.Min((Mathf.Min(arrowScale.x, arrowScale.y)), arrowScale.z);

        var allParticles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            if (ps.name.Contains(meshName))
            {
                ps.gameObject.SetActive(false);
                var main = ps.main;
                main.startSizeMultiplier /= particleScale;

                var shape = ps.shape;
                shape.useMeshColors = false;
                shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
                shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                shape.meshRenderer = meshRenderer;
                ps.gameObject.SetActive(true);

                particles.Add(ps);
            }
        }

        var allMeshes = GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in allMeshes)
        {
            if (meshFilter.name.Contains(meshName))
            {
                meshFilter.sharedMesh = Mesh.GetComponent<MeshFilter>().sharedMesh;
                meshes.Add(meshFilter);
            }
        }

        if (!UseMeshPosition) UpdateEffectsScaleOnStart();
    }

    void ResetEffects()
    {
        foreach (var particle in particles)
        {
            var parent = particle.transform.parent;
            parent.localPosition = Vector3.zero;
            parent.localRotation = new Quaternion();
            parent.localScale = Vector3.one;
        }

        foreach (var meshFilter in meshes)
        {
            var parent = meshFilter.transform.parent;
            parent.localPosition = Vector3.zero;
            parent.localRotation = new Quaternion();
            parent.localScale = Vector3.one;
        }
        particles.Clear();
        meshes.Clear();
        isInitialized = false;
    }

    void UpdateEffects()
    {
        if (!UseMeshPosition) return;
        var meshT = Mesh.transform;
        foreach (var particle in particles)
        {
            var parent = particle.transform.parent;
            parent.position = meshT.position;
            parent.rotation = meshT.rotation;
            parent.localScale = meshT.lossyScale;
        }

        foreach (var meshFilter in meshes)
        {
            var parent = meshFilter.transform.parent;
            parent.position = meshT.position;
            parent.rotation = meshT.rotation;
            parent.localScale = meshT.lossyScale;
        }
    }

    void UpdateEffectsScaleOnStart()
    {
        var meshT = Mesh.transform;
        foreach (var particle in particles)
        {
            var parent = particle.transform.parent;
            parent.localScale = meshT.lossyScale;
        }

        foreach (var meshFilter in meshes)
        {
            var parent = meshFilter.transform.parent;
            parent.localScale = meshT.lossyScale;
        }
    }

    void OnEnable()
    {
        ResetEffects();
    }

    void Update()
    {
        if (Mesh == null) return;

        if (!isInitialized) InitializeEffect();
        UpdateEffects();
    }

    public enum EffectMeshType
    {
        Bow,
        Arrow
    }
}
