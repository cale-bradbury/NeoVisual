using UnityEngine;
using System.Collections;
using System.Reflection;

public class ccEventSetFloat : ccEventBase {

	public float val;
	public CCReflectFloat outValue;


	protected override void OnEvent (){
		outValue.SetValue (val);
	}
}
