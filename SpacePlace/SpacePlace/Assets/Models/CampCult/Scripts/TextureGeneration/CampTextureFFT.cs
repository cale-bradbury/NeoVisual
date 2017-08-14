using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampTextureFFT : MonoBehaviour{

	Texture2D tex;
	public CampReflectTexture output;
    public string globalShader = "";
    public int height = 128;
    public float minimumValue = 0;

	// Use this for initialization
	void OnEnable (){
		tex = new Texture2D (CampAudioController.FFT.Length,height);
		tex.filterMode = FilterMode.Bilinear;
		tex.wrapMode = TextureWrapMode.Clamp;
	}
	
	// Update is called once per frame
	void Update (){
		float[] f = CampAudioController.FFT;
		//float max = getMax (f);
		if (tex.width != f.Length||tex.height!=height)
			OnEnable ();
		Color[] c = tex.GetPixels (0, 0, tex.width, tex.height - 1); 
		tex.SetPixels (0, 1, tex.width, tex.height - 1, c);
		for(int i = 0; i< tex.width; i++){
            float j = Mathf.Lerp(minimumValue, 1,f[i]);//max;
			tex.SetPixel(i,0,new Color(j,j,j));
		}
		tex.Apply ();
		output.SetValue (tex);
        if (globalShader != "")
        {
            Shader.SetGlobalTexture(globalShader, tex);
        }
	}

	float getMax(float[] f){
		float m = 0;
		for(int i = 0; i< f.Length;i++){
			m = Mathf.Max(m,f[i]);
		}
		return m;
	}
}

