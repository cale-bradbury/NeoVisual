using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class CampReflectTexture:CampReflector<Texture>{
	public CampReflectTexture(){
		types = new string[]{typeof(Texture).ToString(),typeof(Texture2D).ToString(),typeof(CampTexture).ToString(),typeof(RenderTexture).ToString()};
	}

	public override void SetValue (object value){
		CheckVar();
		if (isMat) {
			((Material)obj).SetTexture(varName,(Texture)value);
			return;
		}
		if (field != null && field.FieldType == typeof(CampTexture)) {
			((CampTexture)field.GetValue(obj)).texture = (Texture)value;
		} else {
			base.SetValue(value);
		}
	}
}

