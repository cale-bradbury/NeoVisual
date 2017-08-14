using UnityEngine;
using System.Collections;

public class CampCreateOnDestroy : MonoBehaviour {

	public GameObject obj;
	public Transform objParent;

	void OnDestroy(){
		GameObject g = Instantiate<GameObject>(obj);
		if (objParent == null)
			objParent = transform.parent;
		g.transform.parent = objParent;
		g.transform.localPosition = Vector3.zero;
	}
}
