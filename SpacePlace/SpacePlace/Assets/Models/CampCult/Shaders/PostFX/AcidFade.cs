/** \addtogroup PostFX 
 *  @{
 */



using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/AcidFade")]
public class AcidFade : ImageEffectBase {
	
	RenderTexture  aCampumTexture;
	public float sampleDistance = .01f;
	public float sampleFreq = .2f;
	public float fade = .9f;
	public float angle = 0;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (aCampumTexture == null || aCampumTexture.width != source.width || aCampumTexture.height != source.height){
			DestroyImmediate(aCampumTexture);
			aCampumTexture = new RenderTexture(source.width, source.height, 0,RenderTextureFormat.ARGB32);
			aCampumTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit( source, aCampumTexture );
		}
		aCampumTexture.MarkRestoreExpected();
		
		material.SetVector("_x", new Vector4(sampleDistance,sampleFreq,angle*Mathf.PI,fade));
		material.SetTexture("_Last", aCampumTexture);

		Graphics.Blit (source, destination, material);
		Graphics.Blit(destination,aCampumTexture);
	}
}

/** @}*/