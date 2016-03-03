using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class CameraStutter : PostEffectsBase {

    public int frameCount = 3;
    List<RenderTexture> frames;
    public KeyCode key = KeyCode.Space;
    int index = 0;

    // Use this for initialization
    void OnEnable()
    {
        if (frames != null)
        {
            foreach (RenderTexture f in frames)
                f.Release();
            frames.Clear();
            frames = null;
        }       
    }
    public override bool CheckResources()
    {
        return base.CheckResources();
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (frames == null)
        {
            frames = new List<RenderTexture>();
            for (int i = 0; i < frameCount; i++)
            {
                RenderTexture f = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
                frames.Add(f);
            }
        }

        if (Input.GetKey(key))
        {
            Graphics.Blit(frames[index], destination);            
        }
        else
        {
            Graphics.Blit(source, destination);
            Graphics.Blit(source, frames[index]);
        }
        index++;
        index %= frameCount;
    }
}
