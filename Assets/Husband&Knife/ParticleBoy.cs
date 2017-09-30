using UnityEngine;
using System.Collections;

public class ParticleBoy : MonoBehaviour {
    Material mat;
    public float baseSpeed;
    public float randomSpeed;
    public Color color;
    Vector4 phase;
    Vector4 speed;
    float rot = 0;
    public static float direction = Mathf.PI*.5f;
	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
        phase = new Vector4(Random.value, Random.value, Random.value, Random.value) * 100;
        speed = new Vector4(Random.value, Random.value, Random.value, Random.value)*randomSpeed+Vector4.one*baseSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        phase += speed * Time.deltaTime;
        mat.SetVector("_Phase", phase);
        mat.SetColor("_Color", color);
        mat.SetVector("_MainTex_ST", new Vector4(1, 1, phase.w, phase.z));
        float a = Mathf.Atan2(transform.position.x, transform.position.z) + direction;
        transform.localEulerAngles = Vector3.back * Mathf.Rad2Deg*(a);// + (Random.value * 20 - 10));
        //rot += phase.x*.01f;
        //transform.localEulerAngles = Vector3.back * rot;
    }
}
