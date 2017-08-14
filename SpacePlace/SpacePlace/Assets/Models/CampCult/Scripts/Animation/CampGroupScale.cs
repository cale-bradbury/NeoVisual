using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CampCreateGroup))]
public class CampGroupScale : MonoBehaviour {
	public Vector3 axisBaseScale = Vector3.one;
	public Vector3 axisAddScale = Vector3.one;
	public AnimationCurve scale;
	public float phase;
	public float phasePerSecond = 1f;
	public float frequency = 1;
	
	CampCreateGroup group;
	
	// Use this for initialization
	void Start () {
		group = GetComponent<CampCreateGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		phase+=phasePerSecond*Time.deltaTime;
		for(int i = 0; i<group.all.Count;i++){
			group.all[i].transform.localScale = axisBaseScale+axisAddScale*scale.Evaluate((frequency*i/group.all.Count+phase)%1.0f);
		}
	}
}
