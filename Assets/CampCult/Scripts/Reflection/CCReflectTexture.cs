using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class CCReflectTexture:CCReflector<Texture>{
	public CCReflectTexture(){
		types = new string[]{typeof(Texture).ToString(),typeof(Texture2D).ToString(),typeof(CCTexture).ToString(),typeof(RenderTexture).ToString()};
	}

	public override void SetValue (object value){
		CheckVar();
		if (isMat) {
			((Material)obj).SetTexture(varName,(Texture)value);
			return;
		}
		if (field != null && field.FieldType == typeof(CCTexture)) {
			((CCTexture)field.GetValue(obj)).texture = (Texture)value;
		} else {
			base.SetValue(value);
		}
	}
}

