using UnityEngine;
using System.Collections;

public class MidiKnobBankToEvent : MonoBehaviour {
	public int knobCount = 8;
	public int knobStartingIndex = 0;
	public string knobEventPrefix = "k";
	public int knobEventStartingIndex = 0;
	float[] knobs;
	// Use this for initialization
	void Start () {
		knobs = new float[knobCount];
	}
	
	// Update is called once per frame
	void Update () {
		CheckKnob ();
	}
	
	void CheckKnob(){
		for (int i = 0; i< knobs.Length; i++) {
			float f = MidiInput.GetKnob(i+knobStartingIndex,MidiInput.Filter.Fast);
			if(f!=knobs[i]){
				knobs[i] = f;
				Messenger.Broadcast<float>((string)(knobEventPrefix+""+(i+knobEventStartingIndex)),f);
			}
		}
	}
}
