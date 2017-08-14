using UnityEngine;
using System.Collections;

public class CampFireEventOnEnable : MonoBehaviour {

	public float delay = .1f;
	public string eventName;

	// Use this for initialization
	void OnEnable () {
		if(delay==0)
			Fire ();
		else
			Invoke ("Fire", delay);
	}
	void Fire(){
		Messenger.Broadcast (eventName);
	}
}
