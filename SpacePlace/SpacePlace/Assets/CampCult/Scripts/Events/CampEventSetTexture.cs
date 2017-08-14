using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampEventSetTexture : CampEventBase {

	public Texture tex;
	public CampReflectTexture outTexture;


	protected override void OnEvent (){
		outTexture.SetValue (tex);
	}
}
