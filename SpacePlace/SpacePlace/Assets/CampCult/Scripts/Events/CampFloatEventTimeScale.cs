using UnityEngine;
using System.Collections;

public class CampFloatEventTimeScale : CampFloatEventBase {

	protected override void OnEvent (float f){
		Time.timeScale = f;
	}
}
