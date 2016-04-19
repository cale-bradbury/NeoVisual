using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Displacement))]
public class KeySetDisplacementStyle : MonoBehaviour {

    public KeyCode[] keys;
    Displacement dis;
    System.Array values;

	// Use this for initialization
	void Start () {
        dis = GetComponent<Displacement>();
        values = System.Enum.GetValues(typeof(Displacement.MergeType
            ));
    }
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i<keys.Length && i<values.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                dis.merge = (Displacement.MergeType) values.GetValue(i);
            }
        }
	}
}
