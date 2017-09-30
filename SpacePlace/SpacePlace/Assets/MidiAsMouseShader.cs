using UnityEngine;
using System.Collections;

public class MidiAsMouseShader : MonoBehaviour {
    Vector4 mouse = Vector4.zero;
    public int status;
    public int xAxis;
    public int yAxis;
    public int click;
    public float smoothing = .9f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        mouse = Vector4.Lerp(mouse, new Vector4(
            MidiInput.GetKnob(status, xAxis),
            MidiInput.GetKnob(status, yAxis),
            MidiInput.GetKnob(status, click),
            0), smoothing);
        Shader.SetGlobalVector("_Mouse", mouse);
    }
}
