using UnityEngine;
using System.Collections;

public class CampEventOpenURL : CampEventBase {

	public string url;

	protected override void OnEvent ()
	{
		base.OnEvent ();
		Application.OpenURL (url);
	}
}
