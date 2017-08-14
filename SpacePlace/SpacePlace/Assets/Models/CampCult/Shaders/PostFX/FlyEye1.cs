/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/FlyEye1")]
public class FlyEye1 : ImageEffectBase {
	public float    strength = .1f;
	public float	size = 0.1f;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("_Strength", strength);
		material.SetFloat("_Size",size);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/