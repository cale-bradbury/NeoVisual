using UnityEngine;
using System.Collections;

public class CampFireEventOnTrigger : MonoBehaviour {

	public string eventName;
	public Collider obj;
	
	// Update is called once per frame
	void OnTriggerEnter (Collider c) {
		if (obj == null || obj == c) {
			Messenger.Broadcast(eventName);
		}
	}
}
