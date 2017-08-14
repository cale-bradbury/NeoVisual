using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampEventAnimateFloat : CampEventBase {
	public CampReflectFloat field;
	new public AnimationCurve animation;
	public float min = 0;
	public float max = 1;
	public float time = 1;
	float animTime = 1000;
	public bool toggle = false;
	bool fwd = true;
	public string firePostAnimation;
	bool fired = true;

	protected override void OnEvent (){
		fired = false;
		if (!toggle)
			animTime = 0;
		else {
			fwd = !fwd;
			if(!fwd&&animTime>time){
				animTime = time;
			}else if(fwd && animTime<0 ){
				animTime = 0;
			}
		}
		base.OnEvent ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((animTime < time && fwd) || (animTime > 0 && !fwd)) {
			if (fwd)
				animTime += Time.deltaTime;
			else
				animTime -= Time.deltaTime;
			field.SetValue (Mathf.Lerp (min, max, animation.Evaluate (animTime / time)));
		} else if (!fired) {
			fired = true;
			Messenger.Broadcast(firePostAnimation);
		}
	}
}
