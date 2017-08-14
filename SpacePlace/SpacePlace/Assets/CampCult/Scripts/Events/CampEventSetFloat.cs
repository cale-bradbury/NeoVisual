using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampEventSetFloat : CampEventBase {

	public float val;
	public CampReflectFloat outValue;


	protected override void OnEvent (){
		outValue.SetValue (val);
	}
}
