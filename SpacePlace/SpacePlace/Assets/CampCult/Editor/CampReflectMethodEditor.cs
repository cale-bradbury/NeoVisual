using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(CampReflectMethod))]
public class CampReflectMethodEditor : PropertyDrawer{

    List<string> methods;

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
            GetMethods (obj);

			int j = Mathf.Max (0, methods.IndexOf (prop.FindPropertyRelative ("methodName").stringValue));
			int i = EditorGUI.Popup (new Rect (r.x + r.width * .5f, r.y, r.width * .5f, r.height), j, methods.ToArray ());
			if (i < methods.Count)
				prop.FindPropertyRelative ("methodName").stringValue = methods[i];
			else
				prop.FindPropertyRelative ("methodName").stringValue = "";
		}
	}
        
	void GetMethods(object o){
		methods = new List<string> ();
		if (o == null)
			return;
		MethodInfo[] meth = o.GetType ().GetMethods ();
		foreach (MethodInfo m in meth) {
            methods.Add(m.Name);
		}
	}
}

