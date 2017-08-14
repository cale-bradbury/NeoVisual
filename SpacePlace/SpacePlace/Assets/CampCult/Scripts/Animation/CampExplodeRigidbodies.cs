using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampExplodeRigidbodies : MonoBehaviour {

	public List<GameObject> all;
	public float force = 1;
	public float radius = 10;
	
	void OnEnable () {
		foreach(GameObject g in all){
			g.GetComponent<Rigidbody>().AddExplosionForce(force,transform.position,radius);
		}
	}
}
