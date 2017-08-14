using UnityEngine;
using System.Collections;
[System.Serializable]
public class CampReflectColor : CampReflector<Color>
{

	public override void SetValue (object value)
	{
		CheckVar ();
		if (isMat) {
			((Material)obj).SetColor(_varName,(Color)value);
		} else {
			base.SetValue (value);
		}
	}

	public override object GetValue ()
	{
		CheckVar ();
		if (isMat) {
			return ((Material)obj).GetColor(_varName);
		} else {
			return base.GetValue ();
		}
	}

}

