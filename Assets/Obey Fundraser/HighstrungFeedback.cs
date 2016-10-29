using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/Highstrung Feedback")]
public class HighstrungFeedback : ImageEffectBase
{
    public Texture2D audioTexture;
    RenderTexture accumTexture;
    public float amp = .001f;
    public float freq = 200;
    public float freqDividePerLoop = 1;
    public float phaseIncreasePerLoop = 1;
    public Vector4 darken;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
        {
            DestroyImmediate(accumTexture);
            accumTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
            accumTexture.hideFlags = HideFlags.HideAndDontSave;
            Graphics.Blit(source, accumTexture);
        }
        accumTexture.MarkRestoreExpected();

        material.SetVector("darken", darken);
        material.SetVector("shape", new Vector4(amp, freq, freqDividePerLoop, phaseIncreasePerLoop));
        material.SetTexture("_Feed", accumTexture);
        material.SetTexture("_Audio", audioTexture);

        Graphics.Blit(source, destination, material);
        Graphics.Blit(destination, accumTexture);
    }
}
