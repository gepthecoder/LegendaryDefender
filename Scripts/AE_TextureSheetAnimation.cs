using UnityEngine;

public class AE_TextureSheetAnimation : MonoBehaviour
{
    public int TilesX = 4;
    public int TilesY = 4;
    public int FPS = 40;

    public float StartDelay = 0;
    public bool IsLoop = true;
    public bool IsReverse;
    public bool IsInterpolateFrames;
    public bool IsParticleSystemTrail;
    public AE_TextureShaderProperties[] TextureNames = { AE_TextureShaderProperties._MainTex };


    private Renderer currentRenderer;
    private Projector projector;
    private Material instanceMaterial;
    private float animationStartTime;
    private bool canUpdate;
    private int previousIndex;
    private int totalFrames;
    private float currentInterpolatedTime;
    private int currentIndex;
    private Vector2 size;
    private bool isInitialized;
    private bool startDelayIsBroken = false;

    public enum AE_TextureShaderProperties
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

    private void OnEnable()
    {
        if (isInitialized) InitDefaultVariables();
    }

    void OnDisable()
    {
        if (instanceMaterial != null)
        {
            instanceMaterial.DisableKeyword("USE_SCRIPT_FRAMEBLENDING");
        }
    }

    private void Start()
    {
        InitDefaultVariables();
        isInitialized = true;
    }

    void Update()
    {
        if(startDelayIsBroken) ManualUpdate();
    }

    void ManualUpdate()
    {
        if (!canUpdate) return;
        UpdateMaterial();
        SetSpriteAnimation();
        if (IsInterpolateFrames)
            SetSpriteAnimationIterpolated();
    }

    void StartDelayFunc()
    {
        startDelayIsBroken = true;
        animationStartTime = Time.time;
    }

    private void InitDefaultVariables()
    {

        InitializeMaterial();

        totalFrames = TilesX * TilesY;
        previousIndex = 0;
        canUpdate = true;
        var offset = Vector3.zero;
        size = new Vector2(1f / TilesX, 1f / TilesY);
        animationStartTime = Time.time;
        if (StartDelay > 0.00001f)
        {
            startDelayIsBroken = false;
            Invoke("StartDelayFunc", StartDelay);
        }
        else startDelayIsBroken = true;
        if (instanceMaterial != null)
        {
            foreach (var textureName in TextureNames) {
                instanceMaterial.SetTextureScale(textureName.ToString(), size);
                instanceMaterial.SetTextureOffset(textureName.ToString(), offset);
            }
            instanceMaterial.EnableKeyword("USE_SCRIPT_FRAMEBLENDING");
        }
    }
    ParticleSystemRenderer pr;
    private void InitializeMaterial()
    {
        if (IsParticleSystemTrail)
        {
            pr = GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
            currentRenderer = pr;
            instanceMaterial = pr.trailMaterial;
            if (!instanceMaterial.name.EndsWith("(Instance)"))
                instanceMaterial = new Material(instanceMaterial) { name = instanceMaterial.name + " (Instance)" };
            pr.trailMaterial = instanceMaterial;
            return;
        }
        currentRenderer = GetComponent<Renderer>();

        if (currentRenderer == null)
        {
            projector = GetComponent<Projector>();
            if (projector != null)
            {
                if (!projector.material.name.EndsWith("(Instance)"))
                    projector.material = new Material(projector.material) { name = projector.material.name + " (Instance)" };
                instanceMaterial = projector.material;
            }
        }
        else
            instanceMaterial = currentRenderer.material;
    }

    private void UpdateMaterial()
    {
        if (currentRenderer == null)
        {
            if (projector != null)
            {
                if (!projector.material.name.EndsWith("(Instance)"))
                    projector.material = new Material(projector.material) { name = projector.material.name + " (Instance)" };
                instanceMaterial = projector.material;
            }
        }
        else if (IsParticleSystemTrail)
        {
            //pr.trailMaterial = instanceMaterial;
            return;
        }
        else
            instanceMaterial = currentRenderer.material;
    }

    void SetSpriteAnimation()
    {
        int index = (int)((Time.time - animationStartTime) * FPS);
        index = index % totalFrames;

        if (!IsLoop && index < previousIndex)
        {
            canUpdate = false;
            return;
        }

        if (IsInterpolateFrames && index != previousIndex)
        {
            currentInterpolatedTime = 0;
        }
        previousIndex = index;

        if (IsReverse)
            index = totalFrames - index - 1;

        var uIndex = index % TilesX;
        var vIndex = index / TilesX;

        float offsetX = uIndex * size.x;
        float offsetY = (1.0f - size.y) - vIndex * size.y;
        var offset = new Vector2(offsetX, offsetY);

        if (instanceMaterial != null)
        {
            foreach (var textureName in TextureNames)
            {
                instanceMaterial.SetTextureScale(textureName.ToString(), size);
                instanceMaterial.SetTextureOffset(textureName.ToString(), offset);
            }
        }
    }

    private void SetSpriteAnimationIterpolated()
    {
        currentInterpolatedTime += Time.deltaTime;

        var nextIndex = previousIndex + 1;
        //if (nextIndex == totalFrames)
        //    nextIndex = previousIndex + 1;
        if (IsReverse)
            nextIndex = totalFrames - nextIndex + 1;

        var uIndex = nextIndex%TilesX;
        var vIndex = nextIndex/TilesX;

        float offsetX = uIndex*size.x;
        float offsetY = (1.0f - size.y) - vIndex*size.y;
        var offset = new Vector2(offsetX, offsetY);
        if (instanceMaterial != null)
        {
            instanceMaterial.SetVector("_MainTex_NextFrame", new Vector4(size.x, size.y, offset.x, offset.y));
            instanceMaterial.SetFloat("InterpolationValue", Mathf.Clamp01(currentInterpolatedTime*FPS));
        }
    }
}