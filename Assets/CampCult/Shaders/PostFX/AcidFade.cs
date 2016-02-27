using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/AcidFade")]
public class AcidFade : ImageEffectBase {
	
	RenderTexture  accumTexture;
	public float sampleDistance = .01f;
	public float sampleFreq = .2f;
	public float fade = .9f;
	public float angle = 0;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height){
			DestroyImmediate(accumTexture);
			accumTexture = new RenderTexture(source.width, source.height, 0,RenderTextureFormat.ARGB32);
			accumTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit( source, accumTexture );
		}
		accumTexture.MarkRestoreExpected();
		
		material.SetVector("_x", new Vector4(sampleDistance,sampleFreq,angle*Mathf.PI,fade));
		material.SetTexture("_Last", accumTexture);

		Graphics.Blit (source, destination, material);
		Graphics.Blit(destination,accumTexture);
	}
}
