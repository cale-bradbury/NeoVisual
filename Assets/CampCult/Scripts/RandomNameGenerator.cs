using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomNameGenerator : MonoBehaviour {
	public int buttonIndex = 0;
	public float fadeOutDelay = .5f;
	public string[] firstNames;
	public string[] secondNames;
	public string[] thirdNames;
	public Text[] text;
	public float fadeTime = .5f;
	public AnimationCurve fadeOut;
	public AnimationCurve fadeIn;
	bool fading = false;
	float fadeAmount = 0;
	public string showEvent;
	public string hideEvent;
	
	// Use this for initialization

	void OnEnable () {
		Messenger.AddListener ("fuck", CanPlay);
	}
	void CanPlay(){
		fading = false;
	}
	
	void OnEvent (){
		if (!fading) {
			fading = true;
			fadeAmount = 0;
			Messenger.Broadcast(showEvent);
			SetText();
			StartFadeIn();
		}
	}

	void Update(){
		if (Input.GetMouseButtonDown (buttonIndex)) {
			OnEvent ();
		}
	}
	
	void StartFadeOut(){
		if (fadeAmount == 0)
			Messenger.Broadcast (hideEvent);
		fadeAmount += Time.deltaTime / fadeTime;
		fadeAmount = Mathf.Min (1, fadeAmount);
		ColorText (fadeOut.Evaluate (fadeAmount));
		if (fadeAmount == 1) {
			return;
		} else {
			Invoke ("StartFadeOut",0);
		}
	}
	void StartFadeIn(){
		fadeAmount += Time.deltaTime / fadeTime;
		fadeAmount = Mathf.Min (1, fadeAmount);
		ColorText (fadeIn.Evaluate (fadeAmount));
		if (fadeAmount == 1) {
			fadeAmount = 0;
			Invoke ("StartFadeOut",fadeOutDelay);
		} else {
			Invoke ("StartFadeIn",0);
		}
	}

	void ColorText(float f){
		foreach (Text t in text) {
			Color c = t.color;
			c.a = f;
			t.color = c;
		}
	}
	
	void SetText(){
		string a = GetElement (firstNames);
		string b = GetElement (secondNames);
		string c = GetElement (thirdNames);
		if (a == "#" || c.IndexOf (".") == 0|| c.IndexOf (";") != -1)
			a = a + b + c;
		else
			a = a + " " + b + " " + c;

		foreach (Text t in text) {
			t.text = a;
		}
	}
	
	string GetElement(string[] s){
		return s[Mathf.FloorToInt(Random.value*s.Length)];
	}
}