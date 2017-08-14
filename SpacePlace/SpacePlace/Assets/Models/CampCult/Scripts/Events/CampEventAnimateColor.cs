using UnityEngine;
using System.Collections;

public class CampEventAnimateColor : CampEventBase {
	public CampReflectColor output;
	public Color endColor;
	Color startColor;
	public AnimationCurve anim;
	public float animLength = 1;
	public string postEvent;
	float t = 0;

	protected override void OnEvent (){
		t = animLength;
		startColor = (Color)output.GetValue ();
		base.OnEvent ();
	}
	
	// Update is called once per frame
	void Update () {
		if (t > 0) {
			t = Mathf.Max (t-Time.deltaTime,0);
			output.SetValue(Color.Lerp(startColor,endColor,1.0f-anim.Evaluate(t/animLength)));
			if(t==0)
				Messenger.Broadcast(postEvent);
		}
	}
}
