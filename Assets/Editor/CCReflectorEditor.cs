using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(CCReflectTexture))]
[CustomPropertyDrawer(typeof(CCReflectFloat))]
[CustomPropertyDrawer(typeof(CCReflectColor))]
[CustomPropertyDrawer(typeof(CCReflectInt))]
public class CCReflectorEditor : PropertyDrawer{
	List<string> vars;
	public override void OnGUI (Rect rect, SerializedProperty prop, GUIContent label){
		label = EditorGUI.BeginProperty (rect, label, prop);
		Rect r = EditorGUI.PrefixLabel (rect, label);
		Object o = prop.FindPropertyRelative ("obj").objectReferenceValue;
		string s = "";
		if (o != null) {
			s = o.name;
			o.name += "(" + o.GetType () + ")";
		}
		Object obj = (Object)EditorGUI.ObjectField (new Rect (r.x, r.y, r.width * .5f, r.height), o, typeof(Object), true);
		prop.FindPropertyRelative ("obj").objectReferenceValue = obj;
		if(o!=null)o.name = s;
		if (obj != null){
			if(obj.GetType()==typeof(Material)){
				prop.FindPropertyRelative("isMat").boolValue = true;
				GetMatFields ((Material)obj,GetArray(prop.FindPropertyRelative ("types")));
			}else{
				prop.FindPropertyRelative("isMat").boolValue = false;
				GetFields (obj,GetArray(prop.FindPropertyRelative ("types")));
			}
			int j = Mathf.Max (0, vars.IndexOf (prop.FindPropertyRelative ("varName").stringValue));
			int i = EditorGUI.Popup (new Rect (r.x + r.width * .5f, r.y, r.width * .5f, r.height), j, vars.ToArray ());
			if (i < vars.Count)
				prop.FindPropertyRelative ("varName").stringValue = vars [i];
			else
				prop.FindPropertyRelative ("varName").stringValue = "";
		}
	}

	System.Type[] GetArray(SerializedProperty field){
		if (field == null)
			return null;
		System.Type[] o = new System.Type[field.arraySize];
		for(int i = 0; i<field.arraySize;i++){
			o[i] = Utils.GetType(field.GetArrayElementAtIndex(i).stringValue);
		}
		return o;
	}

	void GetMatFields(Material mat, System.Type[] type){
		List<ShaderUtil.ShaderPropertyType> shaderTypes = new List<ShaderUtil.ShaderPropertyType> ();
		foreach (System.Type t in type) {
			if(t==typeof(float)){
				shaderTypes.Add(ShaderUtil.ShaderPropertyType.Float);
				shaderTypes.Add(ShaderUtil.ShaderPropertyType.Range);
				shaderTypes.Add(ShaderUtil.ShaderPropertyType.Vector);
				break;
			}else if(t==typeof(Color)){
				shaderTypes.Add(ShaderUtil.ShaderPropertyType.Color);
				break;
			}else if(t==typeof(Texture)){
				shaderTypes.Add(ShaderUtil.ShaderPropertyType.TexEnv);
				break;
			}
		}
		vars = new List<string> ();
		for (int i = 0; i<ShaderUtil.GetPropertyCount(mat.shader); i++) {
			if (shaderTypes.IndexOf (ShaderUtil.GetPropertyType (mat.shader, i)) != -1) {
				string s = ShaderUtil.GetPropertyName (mat.shader, i);
				if (ShaderUtil.GetPropertyType (mat.shader, i) == ShaderUtil.ShaderPropertyType.Vector) {
					vars.Add (s + "-x");
					vars.Add (s + "-y");
					vars.Add (s + "-z");
					vars.Add (s + "-w");
				} else
					vars.Add (s);
			}
		}
	}

	void GetFields(object o, System.Type[] type){
		vars = new List<string> ();
		if (o == null||type==null)
			return;
		FieldInfo[] fields = o.GetType ().GetFields ();
		foreach (FieldInfo f in fields) {
			foreach(System.Type t in type){
				if(t == f.FieldType){
					if(t==typeof(Vector4)){
						vars.Add(f.Name+"-x");
						vars.Add(f.Name+"-y");
						vars.Add(f.Name+"-z");
						vars.Add(f.Name+"-w");
					}else if(t==typeof(Vector3)){
						vars.Add(f.Name+"-x");
						vars.Add(f.Name+"-y");
						vars.Add(f.Name+"-z");
					}else if(t==typeof(Vector2)){
						vars.Add(f.Name+"-x");
						vars.Add(f.Name+"-y");
					}else{
						vars.Add(f.Name);
					}
					break;
				}
			}
		}
		
		PropertyInfo[] props = o.GetType ().GetProperties ();
		foreach (PropertyInfo p in props) {
			foreach(System.Type t in type){
				if(t == p.PropertyType){
					if(t==typeof(Vector4)){
						vars.Add(p.Name+"-x");
						vars.Add(p.Name+"-y");
						vars.Add(p.Name+"-z");
						vars.Add(p.Name+"-w");
					}else if(t==typeof(Vector3)){
						vars.Add(p.Name+"-x");
						vars.Add(p.Name+"-y");
						vars.Add(p.Name+"-z");
					}else if(t==typeof(Vector2)){
						vars.Add(p.Name+"-x");
						vars.Add(p.Name+"-y");
					}else{
						vars.Add(p.Name);
					}
					break;
				}
			}
		}
	}
}

