using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public float angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ParticleBoy.direction = angle;
	}
}
