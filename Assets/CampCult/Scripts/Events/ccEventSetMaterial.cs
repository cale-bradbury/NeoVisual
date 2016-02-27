using UnityEngine;
using System.Collections;

public class ccEventSetMaterial : ccEventBase {
	public Material mat;

	protected override void OnEvent ()
	{
		GetComponent<Renderer>().material = mat;
	}
}
