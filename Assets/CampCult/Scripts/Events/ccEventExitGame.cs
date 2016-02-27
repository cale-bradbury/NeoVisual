using UnityEngine;
using System.Collections;

public class ccEventExitGame : ccEventBase {


	protected override void OnEvent (){
		Application.Quit ();
	}
}
