using UnityEngine;
using System.Collections;

public class TreeRinger : MonoBehaviour {

    public GameObject treefab;
    public int[] ringCounts;
    public float distancePerRing = 2;
    public float minTreeDistance = 4;
    ccCreateRing[] trees;
    public float phase;
    public float freq;
    public float amp;
    public float arc = 0;

	// Use this for initialization
	void Start () {
        trees = new ccCreateRing[ringCounts.Length];
	    for(int i = 0; i < ringCounts.Length; i++)
        {
            GameObject g = new GameObject("TreeRing-" + i);
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            ccCreateRing r = g.AddComponent<ccCreateRing>();
            r.obj = new GameObject[]{ treefab};
            r.count = ringCounts[i];
            r.radius = distancePerRing*i+minTreeDistance;
            r.arc = arc;
            r.direction = ccCreateRing.Direction.XZ;
            trees[i] = r;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < trees.Length; i++)
        {
            trees[i].phase = Mathf.Sin(phase+i*freq)*amp;
        }
	}
}
