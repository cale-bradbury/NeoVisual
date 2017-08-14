/** \addtogroup PostFX 
*  @{
*/

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/Color Overlay")]
public class ColorOverlay : ImageEffectBase {
	public Color color;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetColor ("_Color", color);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/