using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class CaptureGifEditor : EditorWindow {

	
	public int frames = 100;
	public int frameDelay = 30;
	public bool captureFrames = true;
	public int captureUpscale = 2;
	public string capturePath = "Folder/Name";
	int i = -1;
	int c=0;
	private bool playing = true;
	[MenuItem("Edit/Capture Gif %_g")]

	static void Init () 
	{
		EditorWindow.GetWindow<CaptureGifEditor>();
	}

	void Update()
	{
		c--;
		if(c<=0){
			c = frameDelay;
			if(i>=0){
				Debug.Log("Captured frame "+(frames-i)+"/"+frames);
				EditorApplication.Step();
				if(captureFrames){
					string s = ""+(frames-i);
					while(s.Length<6)s = "0"+s;
					//ScreenCapture.CaptureScreenshot(capturePath+""+s+".png",captureUpscale);
				}
				i--;
			}else if(i==-1){
				EditorApplication.Step();
				i--;
				EditorApplication.isPaused = !playing;
			}
		}
	}

	void capture(){
		playing = EditorApplication.isPlaying;
		EditorApplication.isPaused=true;
		i = frames-1;
	}

	void OnGUI()
	{
		frames = EditorGUILayout.IntField("Frames", frames);
		frameDelay = EditorGUILayout.IntField("Frame Delay", frameDelay);
		captureFrames = GUILayout.Toggle(captureFrames,"Capture Frames");
		if(captureFrames){
			captureUpscale = EditorGUILayout.IntField("Upscale", captureUpscale);
			capturePath = EditorGUILayout.TextField("Path (Path/ExtentionlessName)", capturePath);
		}
		if(GUILayout.Button("Capture"))
		{
			capture();
		}
	}
}
