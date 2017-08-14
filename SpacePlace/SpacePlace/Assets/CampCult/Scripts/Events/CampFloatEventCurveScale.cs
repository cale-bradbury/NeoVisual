using UnityEngine;
using System.Collections;

public class CampFloatEventCurveScale : CampFloatEventBase
{
	public AnimationCurve curve;
	public Vector3 min = Vector3.zero;
	public Vector3 max = Vector3.one;
	
	protected override void OnEvent (float f){
		transform.localScale =  (Vector3.Lerp(min,max, curve.Evaluate (f)));
	}
}
	

