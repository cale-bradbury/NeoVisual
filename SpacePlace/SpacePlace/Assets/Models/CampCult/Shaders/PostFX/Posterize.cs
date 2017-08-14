/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/Posterize")]
public class Posterize : ImageEffectBase {
	public float    steps;
	public float gamma;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("_Steps",steps);
		material.SetFloat("_Gamma",gamma);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/