using UnityEngine;
using System.Collections;

public class CampEventRandomizeAngle : CampEventBase {

    public float smoothing = .1f;
    Vector3 target;
    Vector3 t;

    protected override void OnEnable()
    {
        base.OnEnable();
        t = transform.localEulerAngles;
    }

    protected override void OnEvent()
    {
        base.OnEvent();
        target = Random.onUnitSphere;
    }

    // Update is called once per frame
    void Update () {
        t = Vector3.Slerp(t, target, smoothing);
        transform.LookAt(t);
	}
}
