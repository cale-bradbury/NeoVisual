using UnityEngine;
using System.Collections;

public class ccEventPlaySound : ccEventBase {
	new public AudioSource audio;
	
	// Update is called once per frame
	protected override void OnEvent ()
	{
		audio.Play ();
	}
}
