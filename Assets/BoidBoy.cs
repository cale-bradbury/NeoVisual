using UnityEngine;
using System.Collections;

public class BoidBoy : MonoBehaviour {

    public Boid boidPrefab;
    public int count = 64;
    public Vector3 size;
    Boid[] all;
    public float neighborDist;
    public float maxSpeed;
    public float smoothing;

    // Use this for initialization
    void Start () {
        all = new Boid[count];
        for(int i = 0; i< count; i++)
        {
            Boid g = Instantiate<Boid>(boidPrefab);
            g.transform.parent = transform;
            g.boids = all;
            g.transform.localPosition = new Vector3(Random.value*size.x, Random.value * size.y, Random.value * size.z) - size * .5f;
            all[i] = g;
        }
	}
    int c = 0;
	// Update is called once per frame
	void Update () {
        c++;
        c %= count;
        for (int i = 0; i < count; i++)
        {
            all[i].smoothing = smoothing;
            all[i].neighborDist = neighborDist;
            all[i].maxSpeed = maxSpeed;
            Vector3 v = all[i].transform.localPosition;
            if (v.x > size.x * .5f)
                v.x -= size.x;
            else if (v.x < -size.x * .5f)
                v.x += size.x;

            if (v.z > size.z * .5f)
                v.z -= size.z;
            else if (v.z < -size.z * .5f)
                v.z += size.z;

            if (v.y > size.y * .5f)
                v.y -= size.y;
            else if (v.y < -size.y * .5f)
                v.y += size.y;

            all[i].transform.localPosition = v;
        }
	}
}
