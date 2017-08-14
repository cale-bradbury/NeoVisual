using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Feedback/LastFrame")]
public class CampTextureLastFrame : MonoBehaviour {
    public bool extraFrame = false;
	[HideInInspector]
	public RenderTexture  lastTexture;
	public CampReflectTexture textureOut;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
        if (lastTexture == null || lastTexture.width != source.width || lastTexture.height != source.height){
			DestroyImmediate(lastTexture);
			lastTexture = new RenderTexture(source.width, source.height, 0,RenderTextureFormat.ARGB32);
			lastTexture.hideFlags = HideFlags.HideAndDontSave;
		}
		lastTexture.MarkRestoreExpected();
        if (extraFrame)
        {
            textureOut.SetValue(lastTexture);
            Graphics.Blit(source, lastTexture);
            Graphics.Blit(source, destination);
        }
        else
        {
            Graphics.Blit(source, lastTexture);
            Graphics.Blit(source, destination);
            textureOut.SetValue(lastTexture);
        }
        RenderTexture.active = destination;
        //lastTexture.Release();
        //source.Release();
    }
}
