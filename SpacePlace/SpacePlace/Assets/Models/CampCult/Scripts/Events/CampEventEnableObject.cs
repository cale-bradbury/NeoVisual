using UnityEngine;
using System.Collections;

public class CampEventEnableObject : CampEventBase {
	
	public bool enable = true;
	public GameObject[] obj;

	protected override void OnEvent (){
		base.OnEvent ();
		foreach (GameObject o in obj) {
			o.SetActive(enable);
		}
	}
}
