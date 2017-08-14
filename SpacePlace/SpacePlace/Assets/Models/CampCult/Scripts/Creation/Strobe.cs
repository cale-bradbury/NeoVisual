using UnityEngine;
using System.Collections;
public class Strobe : MonoBehaviour {

	int c=0;
	public int strobeEvery=1;
	MeshRenderer[] mesh;

	void Start(){
		mesh = GetComponentsInChildren<MeshRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if(strobeEvery==0)return;
		c++;
		c%=strobeEvery;
		if(c==0){
			for(int i=0; i< mesh.Length;i++){
				mesh[i].enabled=!mesh[i].enabled;
			}
		}
	}
}
