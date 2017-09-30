/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Color/Saturation")]
public class Saturation : ImageEffectBase {

    public const string keywordWrapUp = "wrapUp";
    public const string keywordWrapDown = "wrapDown";


    public float hue = 0;
	public float sat = 1;
	public float val = 1;
    public bool wrapUp = false;
    bool _wrapUp = false;
    public bool wrapDown = false;
    bool _wrapDown = false;
    public float wrapUpPoint = .01f;
    public float wrapDownPoint = .99f;

    // Called by camera to apply image effect
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        material.SetVector("_Data", new Vector4(hue, sat, val, 0));
        material.SetVector("_Wrap", new Vector4(wrapUpPoint, wrapDownPoint, 0, 0));
        if (wrapUp != _wrapUp)
        {
            _wrapUp = wrapUp;
            if (wrapUp)
                material.EnableKeyword(keywordWrapUp);
            else
                material.EnableKeyword("_"+keywordWrapUp);

        }
        if (wrapDown != _wrapDown)
        {
            _wrapDown = wrapDown;
            if (wrapDown)
                material.EnableKeyword(keywordWrapDown);
            else
                material.EnableKeyword("_" + keywordWrapDown);

        }
        Graphics.Blit (source, destination, material);
	}
}


/** @}*/