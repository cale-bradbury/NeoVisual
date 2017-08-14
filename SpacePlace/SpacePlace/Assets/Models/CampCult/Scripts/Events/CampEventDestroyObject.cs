using UnityEngine;
using System.Collections;

public class CampEventDestroyObject : CampEventBase {

	public GameObject obj;
	
	protected override void OnEvent (){
		base.OnEvent ();
		if (obj == null)
			obj = gameObject;
		Destroy (obj);
	}
}
