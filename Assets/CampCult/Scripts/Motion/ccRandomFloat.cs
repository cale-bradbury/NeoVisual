using UnityEngine;
using System.Collections;

public class ccRandomFloat : MonoBehaviour {
	
	public float min = 0;
	public float max = 1;
	public CCReflectFloat field;

	// Update is called once per frame
	void Update () {
		field.SetValue( Mathf.Lerp (min, max, Random.value));
	}
}
