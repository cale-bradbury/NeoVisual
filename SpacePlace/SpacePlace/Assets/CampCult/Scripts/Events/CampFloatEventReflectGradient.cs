using UnityEngine;
using System.Collections;

public class CampFloatEventReflectGradient : CampFloatEventBase {

    public Gradient gradient;
    public CampReflectColor output;

    protected override void OnEvent(float f)
    {
        base.OnEvent(f);
        output.SetValue(gradient.Evaluate(f));
    }
}
