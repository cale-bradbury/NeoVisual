using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class ccAnimateFloat: ccAnimate{

	public float minValue = 0;
	public float maxValue = 1;
	public CCReflectFloat field;

	public override void Update(){
		base.Update ();
		field.SetValue( Mathf.Lerp(minValue,maxValue,value));
	}
}