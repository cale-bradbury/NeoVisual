/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/Static")]
public class Static : ImageEffectBase {
	public Texture  staticMap;
	public float    strength = .1f;
	public float	sample = 0;
	public float 	samplePerSecond;
	public float	y = 0;
	public float 	yPerSecond;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		sample += samplePerSecond*Time.deltaTime;
		y += yPerSecond*Time.deltaTime;
		material.SetTexture("_StaticTex", staticMap);
		material.SetVector("_x", new Vector4(sample,y,strength,0));
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/