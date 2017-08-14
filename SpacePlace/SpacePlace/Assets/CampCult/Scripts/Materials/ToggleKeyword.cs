using UnityEngine;
using System.Collections;

public class ToggleKeyword : MonoBehaviour {

    public KeyCode key = KeyCode.Space;
    public Material mat;
    public string keyword;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(key))
        {
            if (mat.IsKeywordEnabled(keyword))
            {
                mat.DisableKeyword(keyword);
            }
            else
            {
                mat.EnableKeyword(keyword);
            }
        }
	}
}
