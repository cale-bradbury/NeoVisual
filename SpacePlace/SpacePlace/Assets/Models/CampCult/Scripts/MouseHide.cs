using UnityEngine;
using System.Collections;

public class MouseHide : MonoBehaviour {
    public bool mouseHide = true;
    public bool mouseLock = true;
    void Start () {
        if (mouseHide)
            Cursor.visible = false;
        if (mouseLock)
            Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetMouseButton(0))
        {
            if (mouseHide)
                Cursor.visible = false;
            if (mouseLock)
                Cursor.lockState = CursorLockMode.Locked;
        }
	}
}
