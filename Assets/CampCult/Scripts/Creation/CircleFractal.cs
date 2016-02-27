using UnityEngine;
using System.Collections;

public class CircleFractal : MonoBehaviour {

	public GameObject obj;
	public float minSize = 0.01f;
	public float angleOffset = 0;

	// Use this for initialization
	void Start () {
		create(gameObject,1,angleOffset);
	}

	void create(GameObject par, float size, float a){
		GameObject g = createNew(par,size*0.25f, a);
		GameObject g2 = createNew(par,-size*0.25f, a);
		if(g.transform.lossyScale.x>minSize){
			create (g,size*0.5f,a+angleOffset);
			create (g2,size*0.5f,a+angleOffset);
		}
	}

	GameObject createNew(GameObject par,float offset, float a){
		GameObject g = GameObject.Instantiate(obj) as GameObject;
		g.transform.parent = par.transform;
		Vector3 pos = g.transform.position;
		pos.x+=Mathf.Sin(a)*offset+g.transform.parent.position.x;
		pos.z+=Mathf.Cos(a)*offset+g.transform.parent.position.z;
		pos.y+=0.001f+g.transform.parent.position.y;
		g.transform.position = pos;
		g.transform.Rotate(0,15f,0);
		g.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		float c = 1-pos.y*100;
		g.GetComponent<Renderer>().material.color = new Color(c,c,c);
		return g;
	}
	
	// Update is called once per frame
	void Update () {
		Utils.DestroyAllChildrenIn(gameObject,true);
		angleOffset += Mathf.PI*Time.deltaTime*0.5f;
		create(gameObject,2f,angleOffset);
	}
}
