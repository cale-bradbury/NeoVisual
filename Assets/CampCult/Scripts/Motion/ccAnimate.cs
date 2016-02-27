using UnityEngine;
using System.Collections;

public class ccAnimate: MonoBehaviour {
	public AnimationCurve curve;
	float time = 0;
	public float animationTime = 1;
	protected float value;
	
	void Start (){
		Update ();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(animationTime !=0) time +=  Time.deltaTime/animationTime;
		value = curve.Evaluate(time);
	}
}
