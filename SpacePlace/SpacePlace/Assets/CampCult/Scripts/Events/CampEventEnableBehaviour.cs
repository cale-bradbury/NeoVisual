using UnityEngine;
using System.Collections;

public class CampEventEnableBehaviour : MonoBehaviour {
	
	public string enableEvent;
	public string disableEvent;
	public MonoBehaviour[] behaviours;

	// Use this for initialization
	void OnEnable () {
		Messenger.AddListener(enableEvent,OnEvent);
		Messenger.AddListener(disableEvent,OffEvent);
	}
	void OnDisable () {
		Messenger.RemoveListener(enableEvent,OnEvent);
		Messenger.RemoveListener(disableEvent,OffEvent);
	}
	
	// Update is called once per frame
	void OnEvent () {
		foreach(MonoBehaviour m in behaviours){
			m.enabled = true;
		}
	}
	void OffEvent () {
		foreach(MonoBehaviour m in behaviours){
			m.enabled = false;
		}
	}
}
