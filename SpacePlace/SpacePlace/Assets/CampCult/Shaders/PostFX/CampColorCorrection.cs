/** \addtogroup PostFX 
 *  @{
 */

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/CampColor Correction")]
public class CampColorCorrection : ImageEffectBase {

	public Texture  textureRamp;
	public Vector4 offset;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetTexture ("_RampTex", textureRamp);
		material.SetVector ("_Off", offset);
		Graphics.Blit (source, destination, material);
	}
}

/** @}*/