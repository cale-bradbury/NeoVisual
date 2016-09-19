using UnityEngine;
using System.Collections;

public class ccEventToggleBool : ccEventBase {

    public CCReflectBool toggle;

    protected override void OnEvent()
    {
        base.OnEvent();
        toggle.SetValue(!(bool)toggle.GetValue());
    }
}
