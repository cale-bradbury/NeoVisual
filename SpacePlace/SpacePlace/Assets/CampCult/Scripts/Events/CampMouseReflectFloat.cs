using UnityEngine;
using System.Collections;

public class CampMouseReflectFloat : MonoBehaviour {

	public CampReflectFloat output;
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
