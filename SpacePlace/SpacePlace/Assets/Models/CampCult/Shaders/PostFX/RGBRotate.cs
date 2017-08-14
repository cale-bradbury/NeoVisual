/** \addtogroup PostFX 
*  @{
*/

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/RGBRotate")]
public class RGBRotate : ImageEffectBase {
	public float intensity = 1;
	public float freq = 1;
	public float phase = 0;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat ("_Freq", freq * Mathf.PI * 2);
		material.SetFloat ("_Phase", (phase)*Mathf.PI*2);
		material.SetFloat ("_Intensity", intensity);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/