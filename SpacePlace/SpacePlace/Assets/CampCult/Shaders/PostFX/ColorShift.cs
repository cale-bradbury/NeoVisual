/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/ColorShift")]
public class ColorShift : ImageEffectBase {
	
	RenderTexture  aCampumTexture;
	public float strength = .95f;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (aCampumTexture == null || aCampumTexture.width != source.width || aCampumTexture.height != source.height){
			DestroyImmediate(aCampumTexture);
			aCampumTexture = new RenderTexture(source.width, source.height, 0,RenderTextureFormat.ARGB32);
			aCampumTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit( source, aCampumTexture );
		}
		aCampumTexture.MarkRestoreExpected();
		
		material.SetTexture("iChannel0", aCampumTexture);
		material.SetFloat ("_Strength", strength);
		
		Graphics.Blit (source, destination, material);
		Graphics.Blit(destination,aCampumTexture);
	}
}


/** @}*/