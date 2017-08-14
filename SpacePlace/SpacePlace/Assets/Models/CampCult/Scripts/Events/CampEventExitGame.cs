using UnityEngine;
using System.Collections;

public class CampEventExitGame : CampEventBase {


	protected override void OnEvent (){
		Application.Quit ();
	}
}
