using UnityEngine;
using System.Collections;

public class CampEventPlaySound : CampEventBase {
	new public AudioSource audio;
	
	// Update is called once per frame
	protected override void OnEvent ()
	{
		audio.Play ();
	}
}
