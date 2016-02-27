using UnityEngine;
using System.Collections;

public class ccEventOpenURL : ccEventBase {

	public string url;

	protected override void OnEvent ()
	{
		base.OnEvent ();
		Application.OpenURL (url);
	}
}
