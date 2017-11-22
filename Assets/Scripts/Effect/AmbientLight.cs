using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AmbientLight : MonoBehaviour
{
    public static Color positiveColor;
    public static Color negativeColor;
    static float _intensity;
    public static float intensity
    {
        set
        {
            _intensity = value;
            float c = _intensity * 0.9f;
            positiveColor = new Color(1 - c, 1 - c, 1 - c, 1);
            negativeColor = new Color(c, c, c, 1);
        }
        get
        {
            return _intensity;
        }

    }
    private Material material;

    public Shader ambientLightShader;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(ambientLightShader);
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}