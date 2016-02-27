using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/Displacement")]
public class Displacement : ImageEffectBase {

	public enum MergeType{
		Lerp,
		Add,
		Mul
	}
	public MergeType merge;

	public Texture lastFrame;
	public CCTexture flow;
	public Vector2 offset;
	public float fade = .9f;
	public bool radial = true;
	public float angle = 0;
	public float anglePerSecond = 0;
	public Transform center;
	public bool invertY = true;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		angle += anglePerSecond * Time.deltaTime * Mathf.PI * 2;

		flow.Update ();

		Vector3 p = new Vector3 (center.position.x, center.position.y, center.position.z);
		p = GetComponent<Camera> ().WorldToViewportPoint (p);
		material.SetVector ("_center", new Vector4 (p.x, p.y, p.z, 0.0f));

		material.SetVector ("_x", new Vector4 (offset.x, offset.y, angle, fade));
		material.SetTexture ("_Last", lastFrame);
		material.SetTexture ("_Flow", flow.texture);
		material.SetVector ("_Flow_ST", flow.scaleTranslate);
		
		if (radial) {
			Shader.EnableKeyword ("radial");
			Shader.DisableKeyword ("nradial");
		} else {
			Shader.EnableKeyword ("nradial");
			Shader.DisableKeyword ("radial");
		}
		if (invertY) {
			Shader.EnableKeyword ("invert");
			Shader.DisableKeyword ("ninvert");
		} else {
			Shader.EnableKeyword ("ninvert");
			Shader.DisableKeyword ("invert");
		}
		
		if (merge == MergeType.Add) {
			Shader.EnableKeyword ("madd");
			Shader.DisableKeyword ("mmul");
			Shader.DisableKeyword ("mlerp");
		} else if (merge == MergeType.Mul) {
			Shader.EnableKeyword ("mmul");
			Shader.DisableKeyword ("madd");
			Shader.DisableKeyword ("mlerp");
		} else {
			Shader.EnableKeyword ("mlerp");
			Shader.DisableKeyword ("mmul");
			Shader.DisableKeyword ("madd");
		}

		Graphics.Blit (source, destination, material);
	}
}
