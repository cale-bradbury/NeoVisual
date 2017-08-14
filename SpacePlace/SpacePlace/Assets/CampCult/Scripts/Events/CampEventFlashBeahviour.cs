using UnityEngine;
using System.Collections;

public class CampEventFlashBeahviour : CampEventBase {

    public MonoBehaviour obj;
    public float time = .2f;
    float t;

    protected override void OnEvent()
    {
        base.OnEvent();
        obj.enabled = true;
        t = time;
    }

    // Update is called once per frame
    void Update () {
        t -= Time.deltaTime;
        if (t < Time.deltaTime * .5f)
        {
            obj.enabled = false;
        }
	}
}
