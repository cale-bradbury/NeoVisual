using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class CampAnimateFloat: CampAnimate{

	public float minValue = 0;
	public float maxValue = 1;
	public CampReflectFloat field;

	public override void Update(){
		base.Update ();
		field.SetValue( Mathf.Lerp(minValue,maxValue,value));
	}
}