using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CampCreateGroup))]
public class CampEventRefreshGroup : CampEventBase {

    CampCreateGroup group;
    protected override void OnEnable()
    {
        base.OnEnable();
        group = GetComponent<CampCreateGroup>();
    }

    protected override void OnEvent()
    {
        base.OnEvent();
        group.refresh();
    }
}
