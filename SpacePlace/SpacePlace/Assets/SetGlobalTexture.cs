using UnityEngine;
using System.Collections;

public class SetGlobalTexture : MonoBehaviour {

    public Texture texture;
    public string name;

	// Use this for initialization
	void OnEnable () {
        Shader.SetGlobalTexture(name, texture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
