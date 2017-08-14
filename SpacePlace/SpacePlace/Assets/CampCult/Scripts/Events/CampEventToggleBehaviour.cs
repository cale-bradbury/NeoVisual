using UnityEngine;
using System.Collections;

public class CampEventToggleBehaviour : CampEventBase {

    public MonoBehaviour obj;

    protected override void OnEvent()
    {
        base.OnEvent();
        obj.enabled = !obj.enabled;
    }
}
