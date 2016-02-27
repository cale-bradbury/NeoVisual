using UnityEngine;
using System.Collections;

public class ccFloatEventTimeScale : ccFloatEventBase {

	protected override void OnEvent (float f){
		Time.timeScale = f;
	}
}
