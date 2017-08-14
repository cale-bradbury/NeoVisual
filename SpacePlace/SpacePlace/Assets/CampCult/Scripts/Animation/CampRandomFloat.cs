using UnityEngine;
using System.Collections;

public class CampRandomFloat : MonoBehaviour {
	
	public float min = 0;
	public float max = 1;
	public CampReflectFloat field;
    public bool everyFrame = false;

	// Update is called once per frame
	void Update () {
        if(everyFrame)
		    field.SetValue( Mathf.Lerp (min, max, Random.value));
	}

    void OnEnable()
    {
        field.SetValue(Mathf.Lerp(min, max, Random.value));
    }
}
