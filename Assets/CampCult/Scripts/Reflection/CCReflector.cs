using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

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

    List<string> vars;
    List<string> components;
    public void Draw(Rect r)
    {
        Object o = go;
        if (o == null)
            o = (Object)this.obj;

        Object obj = (Object)EditorGUI.ObjectField(new Rect(r.x, r.y, r.width * .5f, r.height), o, typeof(Object), true);

        if (obj != null)
        {
           isMat = false;
            if (obj.GetType() == typeof(Material))
            {
                go = null;
                this.obj = obj;
                isMat = true;
                GetMatFields((Material)obj, GetArray(types));
                DisplayVarsDropdown( new Rect(r.x + r.width * .5f, r.y, r.width * .5f, r.height));
            }
            else if (obj is GameObject)
            {
                go = (GameObject)obj;
                GetComponents((GameObject)obj);
                int componentIndex = Mathf.Max(0, components.IndexOf(componentName));
                componentIndex = EditorGUI.Popup(new Rect(r.x + r.width * .5f, r.y, r.width * .25f, r.height), componentIndex, components.ToArray());
                componentIndex = Mathf.Clamp(componentIndex, 0, components.Count);
                componentName = components[componentIndex];
                this.obj = ((GameObject)obj).GetComponent(System.Type.GetType(components[componentIndex]));

                GetFields(System.Type.GetType(components[componentIndex]), GetArray(types));
                DisplayVarsDropdown( new Rect(r.x + r.width * .75f, r.y, r.width * .25f, r.height));
            }
            else
            {
                go = null;
                this.obj = obj;
                GetFields(obj.GetType(), GetArray(types));
                DisplayVarsDropdown( new Rect(r.x + r.width * .5f, r.y, r.width * .5f, r.height));
            }
        }
    }

    void DisplayVarsDropdown( Rect r)
    {
        int i = Mathf.Max(0, vars.IndexOf(varName));
        i = EditorGUI.Popup(r, i, vars.ToArray());
        if (i < vars.Count)
            varName = vars[i];
        else
            varName = "";
    }

    System.Type[] GetArray(string[] t)
    {
        System.Type[] o = new System.Type[t.Length];
        for (int i = 0; i < t.Length; i++)
        {
            o[i] = Utils.GetType(t[i]);
        }
        return o;
    }

    void GetMatFields(Material mat, System.Type[] type)
    {
        List<ShaderUtil.ShaderPropertyType> shaderTypes = new List<ShaderUtil.ShaderPropertyType>();
        foreach (System.Type t in type)
        {
            if (t == typeof(float))
            {
                shaderTypes.Add(ShaderUtil.ShaderPropertyType.Float);
                shaderTypes.Add(ShaderUtil.ShaderPropertyType.Range);
                shaderTypes.Add(ShaderUtil.ShaderPropertyType.Vector);
                break;
            }
            else if (t == typeof(Color))
            {
                shaderTypes.Add(ShaderUtil.ShaderPropertyType.Color);
                break;
            }
            else if (t == typeof(Texture))
            {
                shaderTypes.Add(ShaderUtil.ShaderPropertyType.TexEnv);
                break;
            }
        }
        vars = new List<string>();
        for (int i = 0; i < ShaderUtil.GetPropertyCount(mat.shader); i++)
        {
            if (shaderTypes.IndexOf(ShaderUtil.GetPropertyType(mat.shader, i)) != -1)
            {
                string s = ShaderUtil.GetPropertyName(mat.shader, i);
                if (ShaderUtil.GetPropertyType(mat.shader, i) == ShaderUtil.ShaderPropertyType.Vector)
                {
                    vars.Add(s + "-x");
                    vars.Add(s + "-y");
                    vars.Add(s + "-z");
                    vars.Add(s + "-w");
                }
                else
                    vars.Add(s);
            }
        }
    }

    void GetComponents(GameObject g)
    {
        components = new List<string>();
        foreach (Component c in g.GetComponents<Component>())
        {
            if (c != null)
                components.Add(c.GetType().AssemblyQualifiedName);
        }
    }

    void GetFields(System.Type ty, System.Type[] type)
    {
        vars = new List<string>();
        FieldInfo[] fields = ty.GetFields();
        foreach (FieldInfo f in fields)
        {
            foreach (System.Type t in type)
            {
                if (t == f.FieldType)
                {
                    if (t == typeof(Vector4))
                    {
                        vars.Add(f.Name + "-x");
                        vars.Add(f.Name + "-y");
                        vars.Add(f.Name + "-z");
                        vars.Add(f.Name + "-w");
                    }
                    else if (t == typeof(Vector3))
                    {
                        vars.Add(f.Name + "-x");
                        vars.Add(f.Name + "-y");
                        vars.Add(f.Name + "-z");
                    }
                    else if (t == typeof(Vector2))
                    {
                        vars.Add(f.Name + "-x");
                        vars.Add(f.Name + "-y");
                    }
                    else {
                        vars.Add(f.Name);
                    }
                    break;
                }
            }
        }

        PropertyInfo[] props = ty.GetProperties();
        foreach (PropertyInfo p in props)
        {
            foreach (System.Type t in type)
            {
                if (t == p.PropertyType)
                {
                    if (t == typeof(Vector4))
                    {
                        vars.Add(p.Name + "-x");
                        vars.Add(p.Name + "-y");
                        vars.Add(p.Name + "-z");
                        vars.Add(p.Name + "-w");
                    }
                    else if (t == typeof(Vector3))
                    {
                        vars.Add(p.Name + "-x");
                        vars.Add(p.Name + "-y");
                        vars.Add(p.Name + "-z");
                    }
                    else if (t == typeof(Vector2))
                    {
                        vars.Add(p.Name + "-x");
                        vars.Add(p.Name + "-y");
                    }
                    else {
                        vars.Add(p.Name);
                    }
                    break;
                }
            }
        }
    }
}

