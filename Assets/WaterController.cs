using UnityEngine;
using System.Collections;

public class WaterController : MonoBehaviour {
    public RenderTexture water;
    public RenderTexture waterB;
    public Material m;
    public Material waterMaterial;

	// Use this for initialization
	void Start () {
        m.mainTexture = water;
        waterB = new RenderTexture(water.width, water.height, water.depth, water.format);
        Texture2D grey = new Texture2D(1, 1);
        grey.SetPixel(0, 0, Color.grey);
        water.MarkRestoreExpected();
        waterB.MarkRestoreExpected();
        Graphics.Blit(grey, water);
        Graphics.Blit(grey, waterB);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m.mainTexture == water)
        {
            m.mainTexture = waterB;
            Graphics.Blit(waterB, water, m);
            waterMaterial.mainTexture = waterB;
        }
        else
        {
            m.mainTexture = water;
            Graphics.Blit(water, waterB, m);
            waterMaterial.mainTexture = waterB;
        }
        
    }
}
