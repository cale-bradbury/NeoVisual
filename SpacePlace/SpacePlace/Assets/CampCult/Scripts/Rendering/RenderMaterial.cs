using UnityEngine;
using System.Collections;
using System.IO;

public class RenderMaterial : MonoBehaviour {
	
	public int width;
	public int height;

	public Material mat;
	RenderTexture t;
	public string savePath;
	public bool getSeries = true;
	public int numFrames = 100;
	public int startFrame = 0;
	int frames = 0;
	Texture2D t2d;

	// Use this for initialization
	void OnEnable () {
		frames = startFrame;
		if(!getSeries)
			Save(savePath);
	}
	
	// Update is called once per frame
	void Update () {
		if(getSeries&&frames<numFrames)
			Save(savePath+ Utils.PrefixZeros(frames, 8) );
	}

	void UpdateTextures(){
		t = new RenderTexture (width, height,0);
		t2d = new Texture2D(width,height);
	}

	void Save(string p){
		frames++;
		if (t == null || t.width != width || t.height != height)
			UpdateTextures ();
		Graphics.Blit (t, t, mat);

		t2d.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0);
		t2d.Apply();
		string path = Application.dataPath + p+".png";
		Debug.Log("begin save to "+path);
		File.WriteAllBytes(path,t2d.EncodeToPNG());
	}
}
