using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{
    public LookAt look;
    public Transform[] targets;
    public string[] events;
    // Use this for initialization
    void OnEnable()
    {
        for (int i = 0; i < events.Length; i++)
        {
            int j = i;
            Messenger.AddListener(events[i], () =>
            {
                look.lookAt = targets[j];
            });
        }
    }
}
