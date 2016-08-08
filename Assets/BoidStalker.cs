using UnityEngine;
using System.Collections;

public class BoidStalker : MonoBehaviour {

    Displacement d;
    BoidBoy b;
    public float smoothing = .05f;
    Boid target;
    float lastMax = 0;
    public float beatAdd, beatSub, beatMin;
	// Use this for initialization
	void Start () {
        b = FindObjectOfType < BoidBoy> ();
        d = FindObjectOfType<Displacement>();
	}
	
	// Update is called once per frame
	void Update () {
        lastMax -= beatSub;
        lastMax = Mathf.Max(beatMin, lastMax);
        if (ccAudioController.largestValue > lastMax + beatAdd)
        {
            target = b.all[ccAudioController.largestIndex % b.all.Length];
            lastMax = ccAudioController.largestValue;
        }
        transform.position = Vector3.Lerp(transform.position, target.transform.position, smoothing);

    }
}
