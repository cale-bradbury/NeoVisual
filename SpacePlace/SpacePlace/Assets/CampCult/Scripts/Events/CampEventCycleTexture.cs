using UnityEngine;
using System.Collections;

public class CampEventCycleTexture : MonoBehaviour {


    public string eventUp;
    public string eventDown;
    public CampReflectTexture output;
    public Texture[] textures;
    int index = 0;

    void OnEnable()
    {
        Messenger.AddListener(eventUp, OnUp);
        Messenger.AddListener(eventDown, OnDown);
        output.SetValue(textures[0]);
    }

    protected void OnUp()
    {
        index++;
        index %= textures.Length;
        output.SetValue(textures[index]);
    }
    protected void OnDown()
    {
        index--;
        if (index < 0)
            index = textures.Length - 1;
        output.SetValue(textures[index]);
    }

}
