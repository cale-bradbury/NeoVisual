using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class CampAnimateColor: CampAnimate{


	public Color minColor = Color.black;
	public Color maxColor = Color.white;
	public CampReflectColor field;

	public override void Update ()
	{
		base.Update ();
		field.SetValue (Color.Lerp (minColor, maxColor, value));
	}

}