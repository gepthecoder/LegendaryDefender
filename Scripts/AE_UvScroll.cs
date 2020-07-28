using UnityEngine;

[ExecuteInEditMode]
public class AE_UvScroll : MonoBehaviour {

    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public AE_UVTextureShaderProperties[] TextureNames = { AE_UVTextureShaderProperties._MainTex };

    Vector2 uvOffset = Vector2.zero;
    Material mat;

    void Start()
    {
        if(Application.isPlaying)
        mat = GetComponent<Renderer>().material;
        else mat = GetComponent<Renderer>().sharedMaterial;
    }

    void OnWillRenderObject()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        {
            foreach (var textureName in TextureNames)
            {
                mat.SetTextureOffset(textureName.ToString(), uvOffset);
            }
        }
    }


    public enum AE_UVTextureShaderProperties
    {
        _MainTex,
        _DistortTex,
        _Mask,
        _Cutout,
        _CutoutTex,
        _Bump,
        _BumpTex,
        _EmissionTex
    }
}
