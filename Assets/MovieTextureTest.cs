using UnityEngine;
using System.Collections;

public class MovieTextureTest : MonoBehaviour {
    public MovieTexture m;
	// Use this for initialization
	void Start () {
        m.loop = true;
        m.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
