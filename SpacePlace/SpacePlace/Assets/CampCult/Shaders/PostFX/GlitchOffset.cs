/** \addtogroup PostFX 
*  @{
*/

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/GlitchOffset")]
public class GlitchOffset : ImageEffectBase {
	public float    xStrength = .1f;
	public float	yStrength = 1;
	public float	radialStrength = 1;
	public float intensity = 1;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetVector("_Shape", new Vector4(xStrength,yStrength,radialStrength*Mathf.PI*2,0)*intensity);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/