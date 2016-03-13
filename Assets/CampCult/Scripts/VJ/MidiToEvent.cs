using UnityEngine;
using System.Collections;

public class MidiToEvent : MonoBehaviour {
	MidiInput midi;
	public int buttonCount;
	public int knobCount;
	public string buttonEventPrefix = "b";
	public string knobEventPrefix = "k";
	float[] knobs;
	// Use this for initialization
	void Start () {
		knobs = new float[knobCount];
	}
	
	// Update is called once per frame
	void Update () {
		CheckButton ();
		CheckKnob ();
	}

	void CheckButton(){
		for (int i = 0; i< buttonCount; i++) {
			if(MidiInput.GetKeyDown(i)){
				Messenger.Broadcast(buttonEventPrefix+(i+1));
			}
		}
	}

	void CheckKnob(){
		for (int i = 0; i< knobs.Length; i++) {
			float f = MidiInput.GetKnob(i,MidiInput.Filter.Fast);
			if(f!=knobs[i]){
				knobs[i] = f;
				Messenger.Broadcast<float>((string)(knobEventPrefix+""+i),f);
			}
		}
	}
}
