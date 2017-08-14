using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class CampAnimateVector3: CampAnimate{

	public Vector3 minValue = Vector3.zero;
	public Vector3 maxValue = Vector3.one;
	public CampReflectVector3 field;

	public override void Update(){
		base.Update ();
		field.SetValue(Vector3.Lerp(minValue,maxValue,value));
	}
}