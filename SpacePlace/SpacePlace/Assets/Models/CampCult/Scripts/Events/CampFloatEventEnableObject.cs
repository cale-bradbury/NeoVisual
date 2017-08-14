using UnityEngine;
using System.Collections;

public class CampFloatEventEnableObject : MonoBehaviour {
	public string eventName;
	public GameObject[] obj;
	public float min = 0;
	public float max = 1;
	// Use this for initialization
	void OnEnable () {
		Messenger.AddListener<float>(eventName,Fire);
	}
	void OnDisable () {
		Messenger.RemoveListener<float>(eventName,Fire);
	}

	void Fire (float f) {
		int i = Mathf.RoundToInt((f-min)/(max-min)*(obj.Length-1));
		for(int j = 0; j<obj.Length;j++){
			if(j==i) obj[j].SetActive(true);
			else obj[j].SetActive(false);
		}
	}
}
