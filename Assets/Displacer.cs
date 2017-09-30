using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

[ExecuteInEditMode]
public class Displacer : ImageEffectBase {

    public Texture lastFrame;
    RenderTexture lastTexture;
    public Vector4 shape;
    public CCReflectTexture textureOut;
    public bool flipY;
    public KeyCode flipYKey;

    void OnEnable()
    {
        material.SetInt("flip", flipY ? 1 : 0);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetVector("_Shape", shape);
        if (lastFrame != null)
        {
            material.SetTexture("_Feedback", lastFrame);
        }

        if (lastTexture == null || lastTexture.width != source.width || lastTexture.height != source.height)
        {
            DestroyImmediate(lastTexture);
            lastTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
            lastTexture.hideFlags = HideFlags.HideAndDontSave;
        }
        lastTexture.MarkRestoreExpected();
        Graphics.Blit(source, destination, material);
        Graphics.Blit(destination, lastTexture);
        textureOut.SetValue(lastTexture);       
    }

    void Update()
    {
        if (Input.GetKeyDown(flipYKey))
        {
            flipY = !flipY;
            material.SetInt("flip", flipY ? 1 : 0);
        }
    }
}
