using UnityEngine;
using System.Collections;

public class CampEventLockMouse : MonoBehaviour {

	public string lockEvent;
	public string unlockEvent;

	// Use this for initialization
	void OnEnable () {
		Messenger.AddListener(lockEvent,Lock);
		Messenger.AddListener(unlockEvent,Unlock);
	}
	void OnDisable () {
		Messenger.RemoveListener(lockEvent,Lock);
		Messenger.RemoveListener(unlockEvent,Unlock);
	}
	
	// Update is called once per frame
	void Lock () {
		if(lockEvent==unlockEvent)
			Toggle();
		else Cursor.lockState = CursorLockMode.Locked;
	}

	void Unlock(){
		if (lockEvent == unlockEvent)
			Toggle ();
		else Cursor.lockState = CursorLockMode.None;
	}

	void Toggle(){
		if (Cursor.lockState == CursorLockMode.None)
			Cursor.lockState = CursorLockMode.Locked;
		else
			Cursor.lockState = CursorLockMode.None;
	}
}
