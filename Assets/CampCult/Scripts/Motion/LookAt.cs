using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

    public Transform lookAt;
    public float smoothing = .9f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion q = transform.rotation;
        transform.LookAt(lookAt);
        transform.rotation = Quaternion.Lerp(q, transform.rotation, smoothing);
	}
}
