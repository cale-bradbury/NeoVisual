using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class ccAnimateColor: ccAnimate{


	public Color minColor = Color.black;
	public Color maxColor = Color.white;
	public CCReflectColor field;

	public override void Update ()
	{
		base.Update ();
		field.SetValue (Color.Lerp (minColor, maxColor, value));
	}

}