using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class ccAnimateGradient: ccAnimate{


	public Gradient gradient;
	public CCReflectColor field;

	public override void Update ()
	{
		base.Update ();
		field.SetValue (gradient.Evaluate(value));
	}

}