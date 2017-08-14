using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(CampReflectTexture))]
[CustomPropertyDrawer(typeof(CampReflectFloat))]
[CustomPropertyDrawer(typeof(CampReflectColor))]
[CustomPropertyDrawer(typeof(CampReflectInt))]
[CustomPropertyDrawer(typeof(CampReflectBool))]
[CustomPropertyDrawer(typeof(CampReflectVector2))]
[CustomPropertyDrawer(typeof(CampReflectVector3))]
[CustomPropertyDrawer(typeof(CampReflectVector4))]
public class CampReflectorEditor : PropertyDrawer{
	List<string> vars;
    List<string> components;
    public override void OnGUI (Rect rect, SerializedProperty prop, GUIContent label){
		label = EditorGUI.BeginProperty (rect, label, prop);
		Rect r = EditorGUI.PrefixLabel (rect, label);
		Object o = prop.FindPropertyRelative("go").objectReferenceValue;
        if (o == null)
            o = prop.FindPropertyRelative("obj").objectReferenceValue;
        
		Object obj = (Object)EditorGUI.ObjectField (new Rect (r.x, r.y, r.width * .5f, r.height), o, typeof(Object), true);
		
		if (obj != null)
        {
            prop.FindPropertyRelative("isMat").boolValue = false;
            if (obj.GetType()==typeof(Material))
            {
                prop.FindPropertyRelative("go").objectReferenceValue = null;
                prop.FindPropertyRelative("obj").objectReferenceValue = obj;
                prop.FindPropertyRelative("isMat").boolValue = true;
				GetMatFields ((Material)obj,GetArray(prop.FindPropertyRelative ("types")));
                DisplayVarsDropdown(prop, new Rect(r.x + r.width * .5f, r.y, r.width * .5f, r.height));
			}
            else if(obj is GameObject)
            {
                prop.FindPropertyRelative("go").objectReferenceValue = obj;
                GetComponents((GameObject)obj);
                int componentIndex = Mathf.Max(0, components.IndexOf(prop.FindPropertyRelative("componentName").stringValue));
                componentIndex = EditorGUI.Popup(new Rect(r.x + r.width * .5f, r.y, r.width * .25f, r.height), componentIndex, components.ToArray());
                componentIndex = Mathf.Clamp(componentIndex, 0, components.Count);
                prop.FindPropertyRelative("componentName").stringValue = components[componentIndex];
                prop.FindPropertyRelative("obj").objectReferenceValue = ((GameObject)obj).GetComponent(System.Type.GetType(components[componentIndex]));

                GetFields(System.Type.GetType(components[componentIndex]), GetArray(prop.FindPropertyRelative("types")));
                DisplayVarsDropdown(prop, new Rect(r.x + r.width * .75f, r.y, r.width * .25f, r.height));
            }
            else
            {
                prop.FindPropertyRelative("go").objectReferenceValue = null;
                prop.FindPropertyRelative("obj").objectReferenceValue = obj;
                GetFields(obj.GetType(), GetArray(prop.FindPropertyRelative("types")));
                DisplayVarsDropdown(prop, new Rect(r.x + r.width * .5f, r.y, r.width * .5f, r.height));
            }
		}
	}

    void DisplayVarsDropdown(SerializedProperty prop, Rect r)
    {
        int i = Mathf.Max(0, vars.IndexOf(prop.FindPropertyRelative("varName").stringValue));
        i = EditorGUI.Popup(r, i, vars.ToArray());
        if (i < vars.Count)
            prop.FindPropertyRelative("varName").stringValue = vars[i];
        else
            prop.FindPropertyRelative("varName").stringValue = "";
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

    void GetComponents(GameObject g)
    {
        components = new List<string>();
        foreach (Component c in g.GetComponents<Component>())
        {
            if (c!= null)
                components.Add(c.GetType().AssemblyQualifiedName);
        }
    }
    
    void GetFields(System.Type ty, System.Type[] type){
		vars = new List<string> ();
		FieldInfo[] fields = ty.GetFields ();
        foreach (FieldInfo f in fields) {
			foreach(System.Type t in type){
				if(t == f.FieldType){
                    if (type.Length == 1)
                    {
                        vars.Add(f.Name);
                        break;
                    }
                    if (t==typeof(Vector4)){
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
		
		PropertyInfo[] props = ty.GetProperties ();
		foreach (PropertyInfo p in props) {
			foreach(System.Type t in type){
				if(t == p.PropertyType){
                    if (type.Length == 1)
                    {
                        vars.Add(p.Name);
                        break;
                    }
                    if (t==typeof(Vector4)){
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

