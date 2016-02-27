using UnityEngine;
using System.Collections;

public class ccEventSetFullscreen : ccEventBase {

	public Vector2 windowed = new Vector2(960,600);

	protected override void OnEvent ()
	{
		base.OnEvent ();
		Screen.fullScreen = !Screen.fullScreen;
		if(Screen.fullScreen) Screen.SetResolution((int)windowed.x,(int)windowed.y,false);
		else Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height,true);

	}
}
