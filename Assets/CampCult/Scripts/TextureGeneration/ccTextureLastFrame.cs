﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/LastFrame")]
public class ccTextureLastFrame : MonoBehaviour {

	[HideInInspector]
	public RenderTexture  lastTexture;
	public CCReflectTexture textureOut;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (lastTexture == null || lastTexture.width != source.width || lastTexture.height != source.height){
			DestroyImmediate(lastTexture);
			lastTexture = new RenderTexture(source.width, source.height, 0,RenderTextureFormat.ARGB32);
			lastTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit( source, lastTexture );
		}
		lastTexture.MarkRestoreExpected();
		Graphics.Blit(source,destination);
		Graphics.Blit(destination,lastTexture);
		textureOut.SetValue (lastTexture);
	}
}
