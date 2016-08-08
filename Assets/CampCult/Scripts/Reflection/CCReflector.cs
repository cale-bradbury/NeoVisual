using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CCReflector<T>{
	public T type;
	public string[] types = new string[]{typeof(T).ToString()};
	public Object obj;
	protected string _varName;
	public string varName;
	public FieldInfo field;
	public PropertyInfo prop;
	public bool isMat = false;
    public string componentName;
    public GameObject go;

	public void OnEnable(){
		ReloadField ();
	}

	public virtual void ReloadField(){
		_varName = varName;
		isMat = false;
		if (obj.GetType () == typeof(Material)) {
			isMat = true;
			field = null;
			prop = null;
			return;
		}
		field = obj.GetType().GetField(varName);
		if (field == null)
			prop = obj.GetType ().GetProperty (varName);
	}

	public void CheckVar(){
		if ((field == null&&prop==null&&!isMat )||varName!=_varName)
			ReloadField ();
	}
	
	public virtual void SetValue(object value){
		CheckVar();
		if (isMat)
			return;
		if(field!=null)
			field.SetValue (obj, value);
		else if(prop!=null)
			prop.SetValue (obj, value, null);
	}
	
	public virtual object GetValue(){
		CheckVar ();
		if (isMat)
			return null;
		if (field != null)
			return field.GetValue (obj);
		else if (prop != null)
			return prop.GetValue (obj, null);
		return null;
	}
}

