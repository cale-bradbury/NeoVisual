using UnityEngine;
using System.Collections;

public class CampEventCycleTexture : CampEventBase {

    public CampReflectTexture output;
    public Texture[] textures;
    int index = -1;

    void Start()
    {
        OnEvent();
    }

    protected override void OnEvent()
    {
        base.OnEvent();
        index++;
        index %= textures.Length;
        output.SetValue(textures[index]);
    }

}
