using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCycleChildren : MonoBehaviour {

    public string eventUp;
    public string eventDown;

    public GameObject[] objects;
    int index = 0;

    void OnEnable()
    {
        Messenger.AddListener(eventUp, OnUp);
        Messenger.AddListener(eventDown, OnDown);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
        objects[index].SetActive(true);
    }

    protected void OnUp()
    {
        objects[index].SetActive(false);
        index++;
        index %= objects.Length;
        objects[index].SetActive(true);
    }
    protected void OnDown()
    {
        objects[index].SetActive(false);
        index--;
        if (index < 0)
            index = objects.Length - 1;
        objects[index].SetActive(true);
    }
}
