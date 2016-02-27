using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioTrigger : MonoBehaviour {

	//Audio sources
	public AudioSource buttonDown;
	public AudioSource buttonUp;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

		bool down = Input.GetMouseButtonDown(0);
		bool held = Input.GetMouseButton(0);
		bool up = Input.GetMouseButtonUp(0);

		if(down)
		{
			Debug.Log("Button down");
			buttonDown.Play();
		}
		if(held)
		{
			Debug.Log("Button held");
		}
		if(up)
		{
			Debug.Log("Button up");
			buttonUp.Play();
		}	

		if(Input.GetKeyDown("escape"))
		{
			Application.Quit();
		}
	}
}
