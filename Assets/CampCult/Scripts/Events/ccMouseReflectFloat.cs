using UnityEngine;
using System.Collections;

public class ccMouseReflectFloat : MonoBehaviour {

	public CCReflectFloat output;
	public float min;
	public float max;
	public bool xAxis;
    public bool mouseDown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Cursor.visible = false;
        if (!mouseDown || (mouseDown && Input.GetMouseButton(0)))
        {
            if (xAxis)
                output.SetValue(Mathf.Lerp(min, max, Input.mousePosition.x / Screen.width));
            else
                output.SetValue(Mathf.Lerp(min, max, Input.mousePosition.y / Screen.height));
        }

	}
}
