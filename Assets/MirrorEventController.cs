using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Mirror))]
public class MirrorEventController : MonoBehaviour {

    public string xEvent;
    public string yEvent;
    Mirror mirror;

    // Use this for initialization
    void OnEnable()
    {
        mirror = GetComponent<Mirror>();
        Messenger.AddListener(xEvent, ToggleX);
        Messenger.AddListener(yEvent, ToggleY);
    }
    void OnDisable()
    {
        Messenger.RemoveListener(xEvent, ToggleX);
        Messenger.RemoveListener(yEvent, ToggleY);
    }

    void ToggleX()
    {
        mirror.mirrorX = !mirror.mirrorX;
    }
    void ToggleY()
    {
        mirror.mirrorY = !mirror.mirrorY;
    }
}
