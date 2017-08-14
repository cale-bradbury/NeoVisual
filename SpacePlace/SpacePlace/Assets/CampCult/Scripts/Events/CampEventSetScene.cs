using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CampEventSetScene : CampEventBase {

	public string sceneName;

	protected override void OnEvent ()
	{
		base.OnEvent ();
		SceneManager.LoadScene (sceneName);
	}
}
