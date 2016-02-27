using UnityEngine;
using System.Collections;

[System.Serializable]
public class CCTexture{

	public Texture texture;
	[Compact]
	public Vector4 scaleTranslate = new Vector4(1,1,0,0);
	[Compact]
	public Vector4 scaleTranslatePerSecond = Vector4.zero;

	// Update is called once per frame
	public void Update (){
		scaleTranslate += scaleTranslatePerSecond*Time.deltaTime;
	}
}

