using UnityEngine;
using System.Collections;

public class AE_EffectSettingColor : MonoBehaviour
{
    public Color Color = Color.red;
    private Color previousColor;

    void OnEnable()
    {
        UpdateColor();
    }

    void Update()
    {
        if (previousColor != Color)
        {
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        var hue = AE_ColorHelper.ColorToHSV(Color).H;
        AE_ColorHelper.ChangeObjectColorByHUE(gameObject, hue);

        var physicsMotion = GetComponentInChildren<AE_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.HUE = hue;
        previousColor = Color;

    }

}
