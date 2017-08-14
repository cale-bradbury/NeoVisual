using UnityEngine;
using System.Collections;

public class CampAnimate: MonoBehaviour {
	public AnimationCurve curve;
	float time = 0;
	public float animationTime = 1;
    public float startingPhase = 0;
	protected float value;
	
	void Start (){
		Update ();
	}

    void OnEnable()
    {
        time = startingPhase;
    }
	
	// Update is called once per frame
	public virtual void Update () {
		if(animationTime !=0) time +=  Time.deltaTime/animationTime;
		value = curve.Evaluate(time);
	}
}
