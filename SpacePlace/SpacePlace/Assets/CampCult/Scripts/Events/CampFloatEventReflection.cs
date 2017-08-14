using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampFloatEventReflection : CampFloatEventBase {

	public CampReflectFloat obj;
	public bool round = false;
	float v = 0;
	float t = 0;


	void Update(){
		//if (round) {
			if (v != t) {
				v = Mathf.Lerp(v,t,.2f);
				obj.SetValue (v);
			}
		//}
	}

	protected override void OnEvent (float f){
		if (round) {
			t = Mathf.Round(f);
		} else {
			t = f;
		}
	}

}
