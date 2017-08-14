using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampTextureWebcam: MonoBehaviour {
	WebCamTexture tex;
	public KeyCode nextCam;
	public KeyCode prevCam;
	int i = 0;
	public CampReflectTexture output;

	// Use this for initialization
	void Start () {
		tex = new WebCamTexture();
        tex.deviceName = WebCamTexture.devices[0].name;
        tex.Play();
        output.SetValue(tex);
        Debug.Log(WebCamTexture.devices.Length);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(nextCam)){
			i++;
			i%=WebCamTexture.devices.Length;
            tex.Stop();
			tex.deviceName = WebCamTexture.devices[i].name;
			tex.Play();
			output.SetValue(tex);
			Debug.Log("next webcam");
		}else if(Input.GetKeyDown(prevCam)){
			i--;
			if(i<0)i = WebCamTexture.devices.Length-1;
            tex.Stop();
			tex.deviceName = WebCamTexture.devices[i].name;
			tex.Play();
			output.SetValue(tex);
			Debug.Log("prev webcam"+tex.deviceName);
		}
	}
}
