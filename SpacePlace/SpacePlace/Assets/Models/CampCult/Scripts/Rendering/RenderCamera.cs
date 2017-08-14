using UnityEngine;
using System.Collections;
using System.IO;

public class RenderCamera : MonoBehaviour
{

    public int width;
    public int height;

    new public Camera camera;
    Camera display;
    RenderTexture t;
    public string savePath;
    public bool getSeries = true;
    public int numFrames = 100;
    public int startFrame = 0;
    public int frameRate = 30;
    int frames = 0;
    Texture2D t2d;

    // Use this for initialization
    void OnEnable()
    {
        Time.captureFramerate = frameRate;
        GameObject g = new GameObject();
        g.transform.position = camera.transform.position;
        g.name = "DisplayCamera";
        display = g.AddComponent < Camera > ();
        display.CopyFrom(camera);
        UpdateTextures();
        frames = startFrame;
        if (!getSeries)
            Save(savePath);
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest);
        if (getSeries && frames < numFrames) 
            Save(savePath + Utils.PrefixZeros(frames, 8));
    }

    void UpdateTextures()
    {
        
        t = new RenderTexture(width, height, 0);
        t2d = new Texture2D(width, height);
        camera.targetTexture = t;
        DisplayTexture displayTexture = display.gameObject.AddComponent<DisplayTexture>();
        displayTexture.shader = Shader.Find("Unlit/Texture");
        displayTexture.texture = t;
    }

    void Save(string p)
    {
        frames++;
        if (t == null || t.width != width || t.height != height)
            UpdateTextures();
       // camera.Render();

        t2d.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0);
        t2d.Apply();
        string path = Application.dataPath +"/"+ p + ".png";
        Debug.Log("saving image " + path);
        File.WriteAllBytes(path, t2d.EncodeToPNG());
    }
}
