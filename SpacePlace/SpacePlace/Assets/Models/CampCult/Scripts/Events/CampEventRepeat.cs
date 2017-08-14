using UnityEngine;
using System.Collections;

public class CampEventRepeat : MonoBehaviour {

	public string eventToListen;
	public string eventToSend;
	public string eventToStop;
	float delay = 0;
	float[] lastHits;

	// Use this for initialization
	void OnEnable () {
		lastHits = new float[] {0,0,0,0};
		Messenger.AddListener (eventToListen, OnEvent);
		Messenger.AddListener (eventToStop, OnStop);
	}

	void OnDisable(){
		Messenger.RemoveListener (eventToSend, OnEvent);
		Messenger.RemoveListener (eventToStop, OnStop);
	}

	void OnStop(){
		lastHits = new float[] {0,0,0,0};
		CancelInvoke ("Beat");
	}

	void OnEvent(){
		Messenger.Broadcast (eventToSend);
		lastHits [0] = lastHits [1];
		lastHits [1] = lastHits [2];
		lastHits [2] = lastHits [3];
		lastHits [3] = Time.time;
		CancelInvoke ("Beat");
		if (lastHits [0] != 0) {
			delay = ((lastHits[1]-lastHits[0])+
					(lastHits[2]-lastHits[1])+
			         (lastHits[3]-lastHits[2]))/3;
			Invoke ("Beat",delay);
		}
	}

	void Beat(){
		Messenger.Broadcast (eventToSend);
		Invoke ("Beat",delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
