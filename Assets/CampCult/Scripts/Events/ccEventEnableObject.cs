using UnityEngine;
using System.Collections;

public class ccEventEnableObject : ccEventBase {
	
	public bool enable = true;
	public GameObject[] obj;

	protected override void OnEvent (){
		base.OnEvent ();
		foreach (GameObject o in obj) {
			o.SetActive(enable);
		}
	}
}
