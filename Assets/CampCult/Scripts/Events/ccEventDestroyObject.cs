using UnityEngine;
using System.Collections;

public class ccEventDestroyObject : ccEventBase {

	public GameObject obj;
	
	protected override void OnEvent (){
		base.OnEvent ();
		if (obj == null)
			obj = gameObject;
		Destroy (obj);
	}
}
