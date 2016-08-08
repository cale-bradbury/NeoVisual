using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour {
    [HideInInspector]
    public Boid[] boids;
    [HideInInspector]
    public Vector3 vel;
    Vector3 accel = Vector3.zero;
    [HideInInspector]
    public int index;
    public float maxForce = .0003f;
    public float maxSpeed = .000000f;
    public float neighborDist = 2;
    public float smoothing = .98f;
    public Vector3 size;

	// Use this for initialization
	void Start()
    {
        vel = Random.onUnitSphere;
        index %= ccAudioController.FFT.Length;
    }
    [HideInInspector]
    public Vector3 lastVel = Vector3.zero;
    Vector3 lastAccel = Vector3.zero;
    // Update is called once per frame
    void Update () {
        Flock();
        lastAccel = Vector3.Lerp(accel, lastAccel, smoothing);
        vel += lastAccel;
        vel = Vector3.ClampMagnitude(vel, maxSpeed)*Time.deltaTime;
        vel += vel.normalized*(ccAudioController.FFT[index] );
        lastVel = Vector3.Lerp(vel, lastVel, smoothing);
        transform.position += lastVel ;
        
        accel = Vector3.zero;
        transform.localScale = Vector3.one * Mathf.Lerp(.05f, 1f, ccAudioController.FFT[index]);
        
    }

    void Flock()
    {
        Vector3 sep = Seperate();
        Vector3 ali = Align();
        Vector3 coh = Cohesion();
        Vector3 edge = Edge();
        edge *= 5;
        sep*=(1.5f);
        ali*=(1.0f);
        coh*=(1.0f);
        // Add the force vectors to acceleration
        applyForce(edge);
        applyForce(sep);
        applyForce(ali);
        applyForce(coh);
    }
    void applyForce(Vector3 force)
    {
        // We could add mass here if we want A = F / M
        accel+=(force);
    }

    public Vector3 Edge()
    {
        Vector3 e = Vector3.zero;

        e.x = Mathf.Max(0, Mathf.Abs(transform.position.x) - size.x * .5f) * Mathf.Sign(-transform.position.x);
        e.y = Mathf.Max(0, Mathf.Abs(transform.position.y) - size.y * .5f) * Mathf.Sign(-transform.position.y);
        e.z = Mathf.Max(0, Mathf.Abs(transform.position.z) - size.z * .5f) * Mathf.Sign(-transform.position.z);

        return e;
    }

    public Vector3 Seperate()
    {
        float desiredseparation = .80f;
        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;

        for(int i = 0; i<boids.Length; i++)
        {
            if (boids[i] == this)
                continue;
            float d = Vector3.Distance(transform.position, boids[i].transform.position);
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d < desiredseparation))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = transform.position - boids[i].transform.position;
                diff.Normalize();
                diff/=d;        // Weight by distance
                steer+=(diff);
                count++;            // Keep track of how many
            }
        }
        // Average -- divide by how many
        if (count > 0)
            steer /= (float)count;

        // As long as the vector is greater than 0
        if (steer.magnitude > 0)
        {
            // First two lines of code below could be condensed with new PVector setMag() method
            // Not using this method until Processing.js catches up
            // steer.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer*=(maxSpeed);
            steer-=(vel);
            steer = Vector3.ClampMagnitude(steer, maxSpeed);
        }
        return steer;
    }

    public Vector3 Align()
    {
        Vector3 sum = new Vector3(0, 0);
        int count = 0;
        for (int i = 0; i < boids.Length; i++)
        {
            float d = Vector3.Distance(transform.position, boids[i].transform.position);
            if ((d > 0) && (d < neighborDist))
            {
                sum+=(boids[i].vel);
                count++;
            }
        }
        if (count > 0)
        {
            sum/=((float)count);
            // First two lines of code below could be condensed with new PVector setMag() method
            // Not using this method until Processing.js catches up
            // sum.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            sum.Normalize();
            sum*=(maxSpeed);
            Vector3 steer = sum- vel;
            steer = Vector3.ClampMagnitude(steer, maxSpeed);
            return steer;
        }
        else {
            return Vector3.zero;
        }
    }

    Vector3 Cohesion()
    {
        Vector3 sum = Vector3.zero;   // Start with empty vector to accumulate all locations
        int count = 0;
        for (int i = 0; i < boids.Length; i++)
        {
            float d = Vector3.Distance(transform.position, boids[i].transform.position);
            if ((d > 0) && (d < neighborDist))
            {
                sum+=(boids[i].transform.position); // Add location
                count++;
            }
        }
        if (count > 0)
        {
            sum/=(count);
            return seek(sum);  // Steer towards the location
        }
        else {
            return Vector3.zero;
        }
    }

    Vector3 seek(Vector3 target)
    {
        Vector3 desired = target- transform.position;  // A vector pointing from the location to the target
                                                          // Scale to maximum speed
        desired.Normalize();
        desired*=(maxSpeed);

        // Above two lines of code below could be condensed with new PVector setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        Vector3 steer =(desired- vel);
        steer = Vector3.ClampMagnitude(steer, maxSpeed);  // Limit to maximum steering force
        return steer;
    }
}
