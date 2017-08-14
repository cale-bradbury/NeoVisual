using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampCreateGroup : MonoBehaviour {

	public enum SpawnType{
		Linear,
		Random
	}

	public GameObject[] obj;
	public SpawnType spawnType;
	int lastSpawned = -1;
	public int count = 5;
	public bool debug = false;
	[HideInInspector]
	public List<GameObject> all = new List<GameObject>();
	
	public delegate void OnRefresh();
	public OnRefresh onRefresh;
	public delegate void OnCreate(GameObject g);
	public OnCreate onCreate;

	// Use this for initialization
	protected virtual void Start () {


	}

	virtual public Vector3 place(int i){
		return Vector3.zero;
	}

	public void refresh(){
		for(int i =0;i< all.Count;i++){
			all[i].transform.localPosition = place(i);
		}
		if(onRefresh!=null)onRefresh();
	}

	virtual protected void Update(){
		if(all.Count<count){
			for(int i =0;i< count;i++){
				if(all.Count-1<i){
					all.Add(getNew());
				}
				//all[i].transform.localPosition = place(i);
			}
			refresh();
		}else if(all.Count>count){
			for(int i = all.Count-1;i>=0;i--){
				if(i>=count){
					Destroy(all[i]);
					all.RemoveAt(i);
				}else{
					//all[i].transform.localPosition = place(i);
				}
			}
			refresh();
		}
	}

	public GameObject getNew(){
		GameObject g;
		if(spawnType == SpawnType.Linear ){
			lastSpawned++;
			lastSpawned%= obj.Length;
			g = Instantiate(obj[lastSpawned]) as GameObject;
		}else {
			g = Instantiate(obj[(int)Random.Range(0,obj.Length)]) as GameObject;
		}
		g.transform.parent = transform;
		//g.transform.localScale = Vector3.one;
		if(onCreate!=null)onCreate(g);
		return g;
	}
	
	public delegate void apply(GameObject obj, int index);
	
	//function should takea gameObject and an index
	public void applyToAll(apply func){
		for(int i =0;i< all.Count;i++){
			func(all[i] as GameObject,i);
		}
	}

	public GameObject at(int i){
		return all[i] as GameObject;
	}

	public int length(){
		return all.Count;
	}

	void OnDrawGizmos() {
		if (debug) {
			Gizmos.color = Color.yellow;
			for (int i = 0; i<count; i++) {
				Gizmos.DrawSphere (place (i) + transform.position, .5f);
			}
		}
	}
}
