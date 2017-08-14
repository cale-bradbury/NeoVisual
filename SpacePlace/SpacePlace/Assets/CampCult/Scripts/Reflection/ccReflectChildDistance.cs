using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ccReflectChildDistance : MonoBehaviour {

    public CCReflectFloat output;
    public bool incremental = false;
    public float incrementAmount = 1;

	// Use this for initialization
	void Start () {
        System.Type t = output.obj.GetType();
        if (incremental)
        {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
                children.Add(transform.GetChild(i).gameObject);
            children.OrderBy(x => x.transform.localPosition.sqrMagnitude);
            for (int i = 0; i < children.Count; i++)
            {
                output.obj =children[i].GetComponent(t);
                output.SetValue(incrementAmount * i);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                output.obj = transform.GetChild(i).GetComponent(t);
                output.SetValue(transform.GetChild(i).localPosition.sqrMagnitude);
            }
        }
	}
}
