using UnityEngine;
using System.Collections;

public class ParticleBoy : MonoBehaviour {
    Material mat;
    public float baseSpeed;
    public float randomSpeed;
    public Color color;
    Vector4 phase;
    Vector4 speed;
	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
        phase = new Vector4(Random.value, Random.value, Random.value, Random.value) * 100;
        speed = new Vector4(Random.value, Random.value, Random.value, Random.value)*randomSpeed+Vector4.one*baseSpeed;
        transform.localEulerAngles = Vector3.back * Random.value * 360;
    }
	
	// Update is called once per frame
	void Update () {
        phase += speed * Time.deltaTime;
        mat.SetVector("_Phase", phase);
        mat.SetColor("_Color", color);
    }
}
