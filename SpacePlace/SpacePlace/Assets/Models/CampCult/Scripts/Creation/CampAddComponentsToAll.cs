using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CampCreateGroup))]
public class CampAddComponentsToAll : MonoBehaviour {

	public GameObject baseScriptHolder;
	private CampCreateGroup group;
	private Component[] c;

	// Use this for initialization
	void Start () {
		group = GetComponent<CampCreateGroup>();
		group.onCreate += Add;
		c = baseScriptHolder.GetComponents<Component>();
	}
	
	// Update is called once per frame
	void Add (GameObject g) {
		for(int i = 0; i<c.Length;i++){
			CopyComponent(c[i],g);
		}
	}

	Component CopyComponent(Component original, GameObject g){
		System.Type type = original.GetType();
		Component copy = g.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach(System.Reflection.FieldInfo f in fields){
			f.SetValue(copy,f.GetValue(original));
		}
		return copy;
	}
}
