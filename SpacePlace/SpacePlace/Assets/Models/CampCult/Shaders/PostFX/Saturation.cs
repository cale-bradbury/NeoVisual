/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/Saturation")]
public class Saturation : ImageEffectBase {

	public float hue = 0;
	public float sat = 1;
	public float val = 1;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetVector ("_Data", new Vector4 (hue,sat,val,0));
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/