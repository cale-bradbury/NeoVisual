using UnityEngine;
using System.Collections;
using System;

public class CampFireEventOnSystemTime : MonoBehaviour {

	public bool debug = false;
	public int debugHour = 0;
	public int debugMin = 0;

	int h = 0;
	int m = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (debug) {
			if(m!= debugMin){
				m = debugMin;
				h = debugHour;
				Messenger.Broadcast("t"+h+"-"+m);
			}
		} else {
			if(m!= DateTime.Now.Minute){
				m = DateTime.Now.Minute;
				h = DateTime.Now.Hour;
				Messenger.Broadcast("t"+h+"-"+m);
			}
		}
	}
}
