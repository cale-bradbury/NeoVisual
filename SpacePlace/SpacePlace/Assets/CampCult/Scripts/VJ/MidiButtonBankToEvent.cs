using UnityEngine;
using System.Collections;

public class MidiButtonBankToEvent : MonoBehaviour {
	public int buttonCount = 8;
	public int buttonStartingIndex = 0;
	public string buttonEventPrefix = "b";
	public int buttonEventStartingIndex = 0;
	float[] buttons;
	// Use this for initialization
	void Start () {
		buttons = new float[buttonCount];
	}
	
	// Update is called once per frame
	void Update () {
		CheckButton ();
	}
	
	void CheckButton(){
		for (int i = 0; i< buttonCount; i++) {
			if(MidiInput.GetKeyDown(i + buttonStartingIndex))
            {
                Debug.Log("b" + (i + buttonStartingIndex));
                Messenger.Broadcast((string)(buttonEventPrefix+""+(i+buttonEventStartingIndex)));
			}
		}
	}
}
