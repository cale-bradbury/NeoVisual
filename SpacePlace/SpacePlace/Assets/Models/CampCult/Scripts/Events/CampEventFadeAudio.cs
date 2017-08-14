using UnityEngine;
using System.Collections;

public class CampEventFadeAudio : MonoBehaviour {
	
	public string eventName;
	public AudioSource source;
	public float time = 3;
	float cur = 0;
	float start = 0;
	public string postEvent;
	
	// Use this for initialization
	void Start () {
		Messenger.AddListener(eventName,Fire);
	}
	
	// Update is called once per frame
	void Fire () {
		cur = 0;
		start = source.volume;
	}

	void Update(){
		if (cur < time) {
			cur+=Time.smoothDeltaTime;
			source.volume = Mathf.Lerp(start,0,cur/time);
			if(cur>time){
				Messenger.Broadcast(postEvent);
			}
		}
	}
}
