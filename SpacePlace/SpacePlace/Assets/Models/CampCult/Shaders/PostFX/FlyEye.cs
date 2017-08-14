/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/FlyEye")]
public class FlyEye : ImageEffectBase {
	public float    strength = .1f;
	public float	phase = 0;
	public float 	phasePerSecond = 1;
	public float	taps = 3;
	public bool		min = true;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		phase += phasePerSecond * Time.deltaTime;
		float t = Mathf.PI * 2 / taps;
		material.SetFloat("_Strength", strength);
		material.SetFloat("_Phase",(phase*t)%Mathf.PI*2);
		material.SetFloat("_Taps",t);
		if (min)
			Shader.EnableKeyword ("FLYEYE_MIN");
		else
			Shader.DisableKeyword ("FLYEYE_MIN");
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/