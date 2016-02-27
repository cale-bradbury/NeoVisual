using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class CameraWorldParticleFix : MonoBehaviour
{
		void OnPreCull(){
			Shader.SetGlobalMatrix("_Camera2World",GetComponent<Camera>().cameraToWorldMatrix);
		}
}

