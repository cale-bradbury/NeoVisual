using UnityEngine;
using System.Collections;

public class StartupDisplay : MonoBehaviour {

    public int display;

	// Use this for initialization
	void Start () {
        Debug.Log(Display.displays.Length);
       //Display.displays[display].Activate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
