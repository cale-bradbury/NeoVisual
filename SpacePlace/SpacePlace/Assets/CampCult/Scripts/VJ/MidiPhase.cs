using UnityEngine;
using System.Collections;

public class MidiPhase : MonoBehaviour {

	public int midiChannel = 0;
	public AudioSource source;
	public float minPitch = 0;
	public float maxPitch = 1;

	
	// Update is called once per frame
	void Update () {
		source.pitch = MidiInput.GetKnob(midiChannel,MidiInput.Filter.Fast)*(maxPitch-minPitch)+minPitch;
	}
}
