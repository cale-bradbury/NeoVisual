using UnityEngine;
using System.Collections;

public class CampEventToggleBool : CampEventBase {

    public CampReflectBool toggle;

    protected override void OnEvent()
    {
        base.OnEvent();
        toggle.SetValue(!(bool)toggle.GetValue());
    }
}
