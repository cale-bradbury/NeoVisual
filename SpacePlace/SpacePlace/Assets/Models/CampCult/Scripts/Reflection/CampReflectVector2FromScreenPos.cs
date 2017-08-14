using UnityEngine;
using System.Collections;

public class CampReflectVector2FromScreenPos : MonoBehaviour {

    public CampReflectVector2 output;
    public Camera screenCamera;
    public Transform track;
    	

	void Update () {
        Vector3 v = screenCamera.WorldToScreenPoint(track.position);
        Vector2 o = new Vector2(v.x/Screen.width, 1-v.y/Screen.height);
        output.SetValue(o);
	}
}
