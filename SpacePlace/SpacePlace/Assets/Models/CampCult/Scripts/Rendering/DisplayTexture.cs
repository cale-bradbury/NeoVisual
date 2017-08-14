using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class DisplayTexture : ImageEffectBase
{

    public Texture texture;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(texture, destination,material);
    }
}
