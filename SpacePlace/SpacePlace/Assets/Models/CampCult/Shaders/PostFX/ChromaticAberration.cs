/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/Chromatic Aberration")]
public class ChromaticAberration : ImageEffectBase {
	public Vector3 dist;
	public Vector2 center;
	public float intensity = 1;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetVector("_Dist",new Vector4(dist.x,dist.y,dist.z,0)*intensity);
		material.SetVector("_Center",new Vector4(center.x+.5f,center.y+.5f,0,0));
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/