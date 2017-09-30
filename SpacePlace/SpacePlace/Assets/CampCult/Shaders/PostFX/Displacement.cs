/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/Displacement")]
public class Displacement : ImageEffectBase {

	public enum MergeType{
		Lerp,
		Add,
		Mul, 
        Wtf,
        ColorDodge,
        LighterColor,
        VividLight,
        HardMix,
        Difference,
        Subtract,
        Divide
    }
	public MergeType merge;
    MergeType _merge;
    string[] enums;

	public Texture lastFrame;
	public CampTexture flow;
	public Vector4 offsetMinMax;
	public float fade = .9f;
	public bool radial = true;
	public float angle = 0;
	public float anglePerSecond = 0;
	public Transform center;
	public bool invertY = true;
    public Vector4 flowST;
    float extraAngle;

    void OnEnable()
    {
        enums = System.Enum.GetNames(typeof(MergeType));
    }

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		extraAngle += anglePerSecond * Time.deltaTime * Mathf.PI * 2;

		flow.Update ();

		Vector3 p = new Vector3 (center.position.x, center.position.y, center.position.z);
		p = GetComponent<Camera> ().WorldToViewportPoint (p);
		material.SetVector ("_center", new Vector4 (p.x, p.y, p.z, 0.0f));

        material.SetVector("_x", new Vector4(0, 0, angle+extraAngle, fade));
        material.SetVector("_Offset", new Vector4(offsetMinMax.x / Screen.width, offsetMinMax.y / Screen.height, offsetMinMax.z / Screen.width, offsetMinMax.w / Screen.height));
        material.SetTexture ("_Last", lastFrame);
		material.SetTexture ("_Flow", flow.texture);
		material.SetVector ("_FlowST", flowST);
		
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
		
        if(_merge != merge)
        {
            _merge = merge;
            foreach (string s in enums)
            {
                material.DisableKeyword(s);
            }
            material.EnableKeyword(merge.ToString());            
        }

		

		Graphics.Blit (source, destination, material);
	}
}


/** @}*/