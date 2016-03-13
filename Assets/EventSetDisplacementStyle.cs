using UnityEngine;
using System.Collections;

public class EventSetDisplacementStyle : ccEventBase {

    public Displacement.MergeType blendMode;

    Displacement d;

    // Use this for initialization
    void Start () {
        d = FindObjectOfType<Displacement>();
	}

    protected override void OnEvent()
    {
        base.OnEvent();
        d.merge = blendMode;
    }
}
