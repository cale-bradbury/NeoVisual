using UnityEngine;
using System.Collections;

public class midicheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log(MidiInput.GetKey(54));
	}
}
