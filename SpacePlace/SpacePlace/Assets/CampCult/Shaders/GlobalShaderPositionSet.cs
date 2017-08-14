using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class GlobalShaderPositionSet : MonoBehaviour
{
	public string varName  ="_Player";
	public Transform obj;
	public bool screenspace;

	void OnPreCull(){
		if(obj==null)return;
		if(screenspace){
			Vector3 s = GetComponent<Camera>().WorldToScreenPoint(obj.position);
			Shader.SetGlobalVector(varName,new Vector4(s.x,s.y,s.z,0));
		}else
			Shader.SetGlobalVector(varName,new Vector4(obj.position.x,obj.position.y,obj.position.z,0));
	}
}

