/** \addtogroup PostFX 
*  @{
*/

﻿using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class CameraStutter : PostEffectsBase {

    public int frameCount = 3;
    List<RenderTexture> frames;
    List<bool> blitted;
    int index = 0;

    bool stuttering = false;

    // Use this for initialization
    void OnEnable()
    {
        if (frames != null)
        {
            foreach (RenderTexture f in frames)
                f.Release();
            frames.Clear();
            blitted.Clear();
            blitted = null;
            frames = null;
        }       
    }
    public override bool CheckResources()
    {
        return base.CheckResources();
    }

    public void ToggleStutter()
    {
        if (stuttering)
            StopStutter();
        else
            StartStutter();
    }

    public void StartStutter()
    {
        stuttering = true;
    }

    public void StopStutter()
    {
        stuttering = false;
        for (int i = 0; i < blitted.Count; i++)
            blitted[i] = false;
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (frames == null)
        {
            frames = new List<RenderTexture>();
            blitted = new List<bool>();
        }
        if (frames.Count != frameCount)
        {
            if (frames.Count > frameCount&&!stuttering)
            {
                while (frames.Count > frameCount)
                {
                    Destroy(frames[0]);
                    frames.RemoveAt(0);
                    blitted.RemoveAt(0);
                }
            }
            else
            {
                for (int i = frames.Count; i < frameCount; i++)
                {
                    RenderTexture f = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
                    frames.Add(f);
                    blitted.Add(false);
                }
            }
        }

        index++;
        index %= frameCount;
        

        if (stuttering && blitted[index])
        {
            Graphics.Blit(frames[index], destination);            
        }
        else
        {
            Graphics.Blit(source, destination);
            Graphics.Blit(source, frames[index]);
            blitted[index] = true;
        }
    }
}


/** @}*/