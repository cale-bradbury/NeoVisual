using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSwap : ccEventBase {

	public List<GameObject> objects;
	public bool ChangeOnTimer;

	GameObject currentObject;
	int objIndx = 0;

	void Start() {
		currentObject = objects[objIndx];
		currentObject.SetActive(true);
	}

	protected override void OnEvent () {
		if (!ChangeOnTimer) {
			currentObject.SetActive(false);
			objIndx = (objIndx + 1) % objects.Count;
			currentObject = objects[objIndx];
			currentObject.SetActive(true);
		}
	}
}
