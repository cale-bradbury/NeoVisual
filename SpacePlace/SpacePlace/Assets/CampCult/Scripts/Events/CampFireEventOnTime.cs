using UnityEngine;
using System.Collections;

public class CampFireEventOnTime : MonoBehaviour {
	
	public string eventName;
	public float time;
	// Use this for initialization
	void Start () {
		Invoke ("Fire", time);
	}

	void Fire(){
		Messenger.Broadcast (eventName);
		Invoke ("Fire", time);
	}
}
