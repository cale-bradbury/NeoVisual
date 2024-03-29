﻿using UnityEngine;
using System.Collections;

public class BoidBoy : MonoBehaviour {

    public Boid boidPrefab;
    public int count = 64;
    public Vector3 size;
    [HideInInspector]
    public Boid[] all;
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
            g.index = i;
            g.size = size;
            g.transform.localPosition = new Vector3(Random.value*size.x, Random.value * size.y, Random.value * size.z) - size * .5f;
            all[i] = g;
        }
	}

    public void VelcoityFuck()
    {
        for (int i = 0; i < count; i++)
        {
            all[i].lastVel = Random.onUnitSphere * maxSpeed;
        }
    }
    public void VelcoitySuck()
    {
        for (int i = 0; i < count; i++)
        {
            all[i].lastVel = -Vector3.ClampMagnitude(all[i].transform.position, 1)*maxSpeed;
        }
    }

    int c = 0;
	// Update is called once per frame
	void Update () {
        float d;
        for(int i = 0; i<all.Length; i++)
        {
            all[i].neighbors.Clear();
            all[i].dist.Clear();
            for(int j = i+1; j<all.Length; j++)
            {
                d = Vector3.Distance(all[i].transform.position, all[j].transform.position);
                if (d < neighborDist)
                {
                    all[i].neighbors.Add(all[j]);
                    all[j].neighbors.Add(all[i]);
                    all[i].dist.Add(d);
                    all[j].dist.Add(d);
                }
            }
        }

        c++;
        c %= count;
        for (int i = 0; i < count; i++)
        {
            all[i].smoothing = smoothing;
            all[i].neighborDist = neighborDist;
            all[i].maxSpeed = maxSpeed;
            all[i].size = size;
        }
	}
}
