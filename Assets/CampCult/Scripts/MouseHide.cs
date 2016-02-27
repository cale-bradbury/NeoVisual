using UnityEngine;
using System.Collections;

public class MouseHide : MonoBehaviour {
	public string buttonName = "Fire1";
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Cursor.visible = true;
		else if (Input.GetButtonDown (buttonName))
			Cursor.visible = false;
	}
}
