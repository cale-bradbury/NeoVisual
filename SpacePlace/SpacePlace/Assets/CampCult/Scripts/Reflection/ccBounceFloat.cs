using UnityEngine;
using System.Collections;

public class ccBounceFloat : MonoBehaviour {
	
	public CCReflectFloat input;
	public CCReflectFloat output;
	public float multiplier = 1;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		output.SetValue(((float)input.GetValue())*multiplier);
	}
}
