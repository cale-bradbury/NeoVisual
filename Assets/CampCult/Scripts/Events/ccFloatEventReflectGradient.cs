using UnityEngine;
using System.Collections;

public class ccFloatEventReflectGradient : ccFloatEventBase {

    public Gradient gradient;
    public CCReflectColor output;

    protected override void OnEvent(float f)
    {
        base.OnEvent(f);
        output.SetValue(gradient.Evaluate(f));
    }
}
