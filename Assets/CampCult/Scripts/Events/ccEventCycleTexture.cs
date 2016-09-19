using UnityEngine;
using System.Collections;

public class ccEventCycleTexture : ccEventBase {

    public CCReflectTexture output;
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
