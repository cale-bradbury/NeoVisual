/** \addtogroup PostFX 
*  @{
*/

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/Mirror")]
public class Mirror : ImageEffectBase {

    [SerializeField]
    bool _mirrorX;
    public bool mirrorX
    {
        get
        {
            return _mirrorX;
        }
        set
        {
            _mirrorX = value;
            if (_mirrorX)
                material.EnableKeyword("mirrorX");
            else
                material.DisableKeyword("mirrorX");
        }
    }

    [SerializeField]
    bool _mirrorY;
    public bool mirrorY
    {
        get
        {
            return _mirrorY;
        }
        set
        {
            _mirrorY = value;
            if (_mirrorY)
                material.EnableKeyword("mirrorY");
            else
                material.DisableKeyword("mirrorY");
        }
    }

    override protected void Start()
    {
        base.Start();
        mirrorX = mirrorX;
        mirrorY = mirrorY;
    }

    // Called by camera to apply image effect
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        mirrorX = mirrorX;
        mirrorY = mirrorY;
        Graphics.Blit (source, destination, material);
	}
}


/** @}*/