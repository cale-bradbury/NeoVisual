using UnityEngine;
using System.Collections;

public class CampFireEventOnKey : MonoBehaviour {

    [System.Serializable]
    public struct KeyEventPair
    {
        public KeyCode key;
        public string eventName;
    }

    public KeyEventPair[] data;

	// Update is called once per frame
	void Update () {
		for (int i = 0; i<data.Length; i++) {
			if (Input.GetKeyDown (data[i].key))
				Messenger.Broadcast (data[i].eventName);
		}
	}

}


