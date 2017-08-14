/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/2dsdf/LavaLampInvert")]
public class SDFLavaLamp : ImageEffectBase {
	
	public float shape = 0;
	public float sminval = 0.5f;
	public float shapeSize = 0.2f;
	public float gradient = 0.1f;
	public float speed = 1.0f;
	public int shapeCount = 4;

	void Start() {
		material.SetInt("_ShapeCount", shapeCount);
        base.Start();
	}

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("_Shape", shape);
		material.SetFloat("_SMinVal", sminval);
		material.SetFloat("_ShapeSize", shapeSize);
		material.SetFloat("_Gradient", gradient);
		Graphics.Blit (source, destination, material);
	}
}


/** @}*/