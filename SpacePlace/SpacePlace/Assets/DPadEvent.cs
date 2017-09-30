using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPadEvent : MonoBehaviour {
 
    public static bool up;
    public static bool down;
    public static bool left;
    public static bool right;

    float lastX;
    float lastY;
    public string[] events = new string[] {"dUp", "dDown", "dRight", "dLeft" };

    void OnEnable()
    {
        up = down = left = right = false;
        lastX = Input.GetAxis("XDPad");
        lastY = Input.GetAxis("YDPad");
    }

    void Update()
    {
        float x = Input.GetAxis("XDPad");
        float y = Input.GetAxis("YDPad");
        if (x == 1 && lastX != 1) { right = true; } else { right = false; }
        if (x == -1 && lastX != -1) { left = true; } else { left = false; }
        if (y == 1 && lastY != 1) { up = true; } else { up = false; }
        if (y == -1 && lastY != -1) { down = true; } else { down = false; }
        lastX = x;
        lastY = y;
        if (up)
            Messenger.Broadcast(events[0]);
        if (down)
            Messenger.Broadcast(events[1]);
        if (right)
            Messenger.Broadcast(events[2]);
        if (left)
            Messenger.Broadcast(events[3]);
    }
}
