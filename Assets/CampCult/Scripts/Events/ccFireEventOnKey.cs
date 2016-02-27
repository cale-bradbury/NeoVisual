using UnityEngine;
using System.Collections;

public class ccFireEventOnKey : MonoBehaviour {

	public KeyCode[] key;
	public string[] eventName;

	// Update is called once per frame
	void Update () {
		for (int i = 0; i<key.Length; i++) {
			if (Input.GetKeyDown (key[i]))
				Messenger.Broadcast (eventName[i]);
		}
	}
}
