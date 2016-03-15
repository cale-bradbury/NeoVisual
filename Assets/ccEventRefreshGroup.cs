using UnityEngine;
using System.Collections;
[RequireComponent(typeof(ccCreateGroup))]
public class ccEventRefreshGroup : ccEventBase {

    ccCreateGroup group;
    protected override void OnEnable()
    {
        base.OnEnable();
        group = GetComponent<ccCreateGroup>();
    }

    protected override void OnEvent()
    {
        base.OnEvent();
        group.refresh();
    }
}
