using System;
using UnityEngine;

// This class implements simple ghosting type Motion Blur.
// If Extra Blur is selected, the scene will allways be a little blurred,
// as it is scaled to a smaller resolution.
// The effect works by aCampumulating the previous frames in an aCampumulation
// texture.
namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Blur/Motion Blur (Color ACampumulation)")]
    [RequireComponent(typeof(Camera))]
    public class MotionBlur : ImageEffectBase
    {
        [Range(0.0f, 0.92f)]
        public float blurAmount = 0.8f;
        public bool extraBlur = false;

        private RenderTexture aCampumTexture;

        override protected void Start()
        {
            if (!SystemInfo.supportsRenderTextures)
            {
                enabled = false;
                return;
            }
            base.Start();
        }

        override protected void OnDisable()
        {
            base.OnDisable();
            DestroyImmediate(aCampumTexture);
        }

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            // Create the aCampumulation texture
            if (aCampumTexture == null || aCampumTexture.width != source.width || aCampumTexture.height != source.height)
            {
                DestroyImmediate(aCampumTexture);
                aCampumTexture = new RenderTexture(source.width, source.height, 0);
                aCampumTexture.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit( source, aCampumTexture );
            }

            // If Extra Blur is selected, downscale the texture to 4x4 smaller resolution.
            if (extraBlur)
            {
                RenderTexture blurbuffer = RenderTexture.GetTemporary(source.width/4, source.height/4, 0);
                aCampumTexture.MarkRestoreExpected();
                Graphics.Blit(aCampumTexture, blurbuffer);
                Graphics.Blit(blurbuffer,aCampumTexture);
                RenderTexture.ReleaseTemporary(blurbuffer);
            }

            // Clamp the motion blur variable, so it can never leave permanent trails in the image
            blurAmount = Mathf.Clamp( blurAmount, 0.0f, 0.92f );

            // Setup the texture and floating point values in the shader
            material.SetTexture("_MainTex", aCampumTexture);
            material.SetFloat("_ACampumOrig", 1.0F-blurAmount);

            // We are aCampumulating motion over frames without clear/discard
            // by design, so silence any performance warnings from Unity
            aCampumTexture.MarkRestoreExpected();

            // Render the image using the motion blur shader
            Graphics.Blit (source, aCampumTexture, material);
            Graphics.Blit (aCampumTexture, destination);
        }
    }
}
