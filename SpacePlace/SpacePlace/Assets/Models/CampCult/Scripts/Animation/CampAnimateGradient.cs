using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class CampAnimateGradient: CampAnimate{


	public Gradient gradient;
	public CampReflectColor field;

	public override void Update ()
	{
		base.Update ();
		field.SetValue (gradient.Evaluate(value));
	}

}