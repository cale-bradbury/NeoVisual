using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/SimpleDisplacement")]
public class SimpleDisplacement : ImageEffectBase
{
    public Texture displacementMap;
    public float distanceX = 1;
    public float distanceY = 1;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetVector("_Strength", new Vector4(distanceX, distanceY, 0, 0));
        material.SetTexture("_DisplacementTex", displacementMap);
        Graphics.Blit(source, destination, material);
    }
}