using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampEventCallFunction : CampEventBase {


    public CampReflectMethod output;

    protected override void OnEvent()
    {
        base.OnEvent();
        output.Invoke();
    }
}
