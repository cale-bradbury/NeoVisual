/** \addtogroup PostFX 
*  @{
*/

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class Upfest : ImageEffectBase {
	public Color c1top;
	public Color c1bot;
	public Color c2top;
	public Color c2bot;
	public float amp = 0;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetColor ("_Color1", c1top);
		material.SetColor ("_Color12", c1bot);
		material.SetColor ("_Color2", c2top);
		material.SetColor ("_Color22", c2bot);
		material.SetFloat ("_Amp", amp);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/