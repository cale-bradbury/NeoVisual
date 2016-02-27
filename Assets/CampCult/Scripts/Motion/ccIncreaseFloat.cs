using UnityEngine;
using System.Collections;

public class ccIncreaseFloat : MonoBehaviour {

	public CCReflectFloat output;
	public float speed;
	float v;

	
	// Update is called once per frame
	void Update () {
		v += speed * Time.deltaTime;
		output.SetValue (v);
	}
}
