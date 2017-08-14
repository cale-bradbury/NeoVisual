using UnityEngine;
using System.Collections;

public class CampChildScaleDistance : MonoBehaviour {

    public float distance = 0;
    Vector3[] pos;
    public Transform scaleWith;

	// Use this for initialization
	void Start () {
        pos = new Vector3[transform.childCount];
        for(int i = 0; i<transform.childCount; i++)
        {
            pos[i] = transform.GetChild(i).localPosition;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = pos[i]*(distance+1);
        }
        scaleWith.localScale = transform.localScale = Vector3.one * (distance + 1);
    }
}
