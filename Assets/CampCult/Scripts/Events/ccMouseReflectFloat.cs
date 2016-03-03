using UnityEngine;
using System.Collections;

public class ccMouseReflectFloat : MonoBehaviour {

	public CCReflectFloat output;
	public float min;
	public float max;
	public bool xAxis;
    public bool mouseDown;
	public float smoothing = .9f;
	Vector3 pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
=======
		Cursor.visible = false;
>>>>>>> e4a8008a65d320339ca22684d618c5abb72f3f7b
        if (!mouseDown || (mouseDown && Input.GetMouseButton(0)))
        {
			pos = Vector3.Lerp(Input.mousePosition, pos, smoothing);
            if (xAxis)
                output.SetValue(Mathf.Lerp(min, max, pos.x / Screen.width));
            else
                output.SetValue(Mathf.Lerp(min, max, pos.y / Screen.height));
        }

	}
}
