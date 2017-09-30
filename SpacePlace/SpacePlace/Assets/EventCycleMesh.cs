using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCycleMesh : MonoBehaviour {

    public string eventUp;
    public string eventDown;

    public Mesh[] meshes;
    public MeshFilter meshFilter;
    int index = 0;

    void OnEnable()
    {
        Messenger.AddListener(eventUp, OnUp);
        Messenger.AddListener(eventDown, OnDown);
    }

    protected void OnUp()
    {
        index++;
        index %= meshes.Length;
        meshFilter.mesh = meshes[index];
    }
    protected void OnDown()
    {
        index--;
        if (index < 0) 
            index = meshes.Length-1;
        meshFilter.mesh = meshes[index];
    }

}
