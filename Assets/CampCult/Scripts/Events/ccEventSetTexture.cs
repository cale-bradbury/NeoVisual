using UnityEngine;
using System.Collections;
using System.Reflection;

public class ccEventSetTexture : ccEventBase {

	public Texture tex;
	public CCReflectTexture outTexture;


	protected override void OnEvent (){
		outTexture.SetValue (tex);
	}
}
