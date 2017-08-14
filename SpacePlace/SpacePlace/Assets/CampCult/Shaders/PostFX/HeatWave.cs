/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/HeatWave")]
public class HeatWave : ImageEffectBase {
	public Texture  depthMap;
	public float    strength = .1f;
	public float	phase = 0;
	public float 	phasePerSecond = 1;
	public float	freq = 1;
	public float	taps = 3;
	public bool radial = false;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		phase += phasePerSecond * Time.deltaTime;
		float t = Mathf.PI * 2 / taps;
		material.SetTexture("_DepthMap", depthMap);
		material.SetFloat("_Strength", strength);
		material.SetFloat("_Phase",(phase*t)%Mathf.PI*2);
		material.SetFloat("_Taps",t);
		material.SetFloat("_Freq",freq);
		if (radial)
			Shader.EnableKeyword ("HEATWAVE_RADIAL");
		else
			Shader.DisableKeyword ("HEATWAVE_RADIAL");
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/