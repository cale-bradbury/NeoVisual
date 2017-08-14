using UnityEngine;
using System.Collections;

public class CampBindRotation : MonoBehaviour {

	public Transform bind;
	public bool bindX=true;
	public bool bindY = true;
	public bool bindZ = true;
	public bool smoothing = false;
	public float smooth = .5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = transform.eulerAngles;
		if(smoothing){
			if(bindX)v.x = Mathf.Lerp(v.x, bind.eulerAngles.x, smooth*Time.deltaTime);
			if(bindY)v.y = Mathf.Lerp(v.y, bind.eulerAngles.y, smooth*Time.deltaTime);
			if(bindZ)v.z = Mathf.Lerp(v.z, bind.eulerAngles.z, smooth*Time.deltaTime);
		}else{
			if(bindX)v.x = bind.eulerAngles.x;
			if(bindY)v.y = bind.eulerAngles.y;
			if(bindZ)v.z = bind.eulerAngles.z;
		}
		transform.eulerAngles = v;
	}
}
