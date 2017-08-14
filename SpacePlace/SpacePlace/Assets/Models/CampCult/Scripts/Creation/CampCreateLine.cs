using UnityEngine;
using System.Collections;

public class CampCreateLine : CampCreateGroup {

	private Vector3 _offset;
	public Vector3 offsetPerObject;

	override public Vector3 place(int i){
		return offsetPerObject*i;
	}

	protected override void Start(){
		base.Start();
		_offset = offsetPerObject;
	}

	protected override void Update(){
		base.Update();
		if(_offset!=offsetPerObject){
			_offset = offsetPerObject;
			refresh();
		}
	}
}
