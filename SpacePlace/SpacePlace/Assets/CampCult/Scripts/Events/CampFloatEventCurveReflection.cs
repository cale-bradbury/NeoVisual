using UnityEngine;
using System.Collections;

public class CampFloatEventCurveReflection : CampFloatEventBase
{

	public CampReflectFloat output;
	public AnimationCurve curve;
	public float min = 0;
	public float max = 1;
	float t;

	protected override void OnEvent (float f){
		t = f;
	}

	void Update(){
		output.SetValue (Utils.Lerp(min,max, curve.Evaluate (t)));
	}
}

