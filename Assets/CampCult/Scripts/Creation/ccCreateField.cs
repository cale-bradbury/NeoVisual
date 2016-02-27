using UnityEngine;
using System.Collections;

public class ccCreateField : ccCreateGroup {

	public Vector3 distance;
	private Vector3 _distance;

	public bool center = true;
	private bool _center;

	public override Vector3 place (int i)
	{
		Vector3 p  = new Vector3(Random.Range(0,distance.x),Random.Range(0,distance.y),Random.Range(0,distance.z));
		if(center){
			p -= distance*.5f;
		}
		return p;
	}

	// Use this for initialization
	new void Start () {
		base.Start();
		_distance = distance;
		_center = center;
	}
	
	// Update is called once per frame
	
	override protected void Update () {
		base.Update();
		if(_distance!=distance){
			_distance = distance;
			refresh();
		}
		if(_center!=center){
			_center = center;
			refresh();
		}
	}
}
