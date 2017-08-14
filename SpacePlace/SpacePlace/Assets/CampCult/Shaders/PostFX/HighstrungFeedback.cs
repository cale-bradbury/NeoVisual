/** \addtogroup PostFX 
*  @{
*/

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/Highstrung Feedback")]
public class HighstrungFeedback : ImageEffectBase
{
    public Texture2D audioTexture;
    RenderTexture accumTexture;
    public float phase = .001f;
    public Vector4 _Freq = Vector4.one*200;
    public float phaseIncreasePerLoop = 1;
    public Vector4 darken;
    float p;
    public float pushFromCenter = .01f;
    public Vector2 center;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
        {
            DestroyImmediate(accumTexture);
            accumTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
            accumTexture.hideFlags = HideFlags.HideAndDontSave;
            accumTexture.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, accumTexture);
        }
        accumTexture.MarkRestoreExpected();

        material.SetVector("darken", darken);
        p += phase;
        material.SetVector("_Center", center);
        material.SetVector("shape", new Vector4(phase, p,0, phaseIncreasePerLoop));
        material.SetTexture("_Feed", accumTexture);
        material.SetTexture("_Audio", audioTexture);

        material.SetVector("_Freq", _Freq);
        material.SetFloat("_Amp", pushFromCenter);

        Graphics.Blit(source, destination, material);
        Graphics.Blit(destination, accumTexture);
    }
}


/** @}*/