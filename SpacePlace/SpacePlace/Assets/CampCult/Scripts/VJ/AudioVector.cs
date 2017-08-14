using UnityEngine;
using System.Collections;

public class AudioVector : MonoBehaviour
{
	public string vectorName;
	public Material mat;
	public bool xOn;
	public bool yOn;
	public bool zOn;
	public bool wOn;
	public Vector4 minValue;
	public Vector4 maxValue;
	public bool additive;
	public float smoothing = .001f;
	public float beat = .1f;
	Vector4 vector;
	float v = 0;

	// Use this for initialization
	void Start ()
	{
		vector = Vector4.zero;//mat.GetVector(vectorName);
		mat = GetComponent<Renderer>().material;
	}

	// Update is called once per frame
	void Update ()
	{
		v-=smoothing;
		v = Mathf.Max (0,v);
		if(CampAudioController.value>v+beat)v = CampAudioController.value;
		Debug.Log(CampAudioController.value);
		vector = mat.GetVector(vectorName);
			if(xOn)vector.x = (maxValue.x - minValue.x)*v+minValue.x;
			if(yOn)vector.y = (maxValue.y - minValue.y)*v+minValue.y;
			if(zOn)vector.z = (maxValue.z - minValue.z)*v+minValue.z;
			if(wOn)vector.w = (maxValue.w - minValue.w)*v+minValue.w;

		mat.SetVector(vectorName, vector);
	}
}

