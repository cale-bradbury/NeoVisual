using UnityEngine;
using System.Collections;

public class ccEventCycleDisplacementStyle : ccEventBase {
    Displacement d;
    public Displacement.MergeType[] styles;
    int i;

	// Use this for initialization
	void Start () {
        i = -1;
        d = FindObjectOfType<Displacement>();
        OnEvent();
	}
    protected override void OnEvent()
    {
        base.OnEvent();
        i++;
        i %= styles.Length;
        d.merge = styles[i];
    }
}
