/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/Ripple")]
public class Ripple : ImageEffectBase {
	public float    strength = .05f;
	public float speed  =1;
	public float freq = 1;
	float	phase;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		phase+=speed*Time.deltaTime;
		material.SetFloat("_Strength", strength);
		material.SetFloat("_Phase",phase);
		material.SetFloat("_Freq",freq);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/