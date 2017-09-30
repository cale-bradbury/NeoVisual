using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCycleDisplacement : MonoBehaviour {

    public string eventUp;
    public string eventDown;

    public Displacement.MergeType[] type;
    public Displacement displacement;
    int index = 0;

    void OnEnable()
    {
        Messenger.AddListener(eventUp, OnUp);
        Messenger.AddListener(eventDown, OnDown);
    }

    protected void OnUp()
    {
        index++;
        index %= type.Length;
        displacement.merge = type[index];
    }
    protected void OnDown()
    {
        index--;
        if (index < 0)
            index = type.Length - 1;
        displacement.merge = type[index];
    }
}
