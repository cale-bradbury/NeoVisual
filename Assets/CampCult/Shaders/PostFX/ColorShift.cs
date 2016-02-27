using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/ColorShift")]
public class ColorShift : ImageEffectBase {
	
	RenderTexture  accumTexture;
	public float strength = .95f;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height){
			DestroyImmediate(accumTexture);
			accumTexture = new RenderTexture(source.width, source.height, 0,RenderTextureFormat.ARGB32);
			accumTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit( source, accumTexture );
		}
		accumTexture.MarkRestoreExpected();
		
		material.SetTexture("iChannel0", accumTexture);
		material.SetFloat ("_Strength", strength);
		
		Graphics.Blit (source, destination, material);
		Graphics.Blit(destination,accumTexture);
	}
}
