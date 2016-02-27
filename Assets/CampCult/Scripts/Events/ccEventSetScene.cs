using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ccEventSetScene : ccEventBase {

	public string sceneName;

	protected override void OnEvent ()
	{
		base.OnEvent ();
		SceneManager.LoadScene (sceneName);
	}
}
