using UnityEngine;
using System.Collections;

public class CampEventSetMaterial : CampEventBase {
	public Material mat;

	protected override void OnEvent ()
	{
		GetComponent<Renderer>().material = mat;
	}
}
