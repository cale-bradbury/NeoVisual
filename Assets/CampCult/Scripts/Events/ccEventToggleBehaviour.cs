using UnityEngine;
using System.Collections;

public class ccEventToggleBehaviour : ccEventBase {

    public MonoBehaviour obj;

    protected override void OnEvent()
    {
        base.OnEvent();
        obj.enabled = !obj.enabled;
    }
}
