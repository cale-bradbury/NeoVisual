using UnityEngine;
using System.Collections;
using System.Reflection;

public class ccEventCallFunction : ccEventBase {


    public ccReflectMethod output;

    protected override void OnEvent()
    {
        base.OnEvent();
        output.Invoke();
    }
}
