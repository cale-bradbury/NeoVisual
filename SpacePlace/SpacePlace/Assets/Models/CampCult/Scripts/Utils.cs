using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Utils {
	public static float Lerp (float min, float max, float f){
		return min+(max-min)*f;
	}

    public static string VerticalText(string input) {
        var sb = new System.Text.StringBuilder(input.Length * 2);
            for (var i = 0; i<input.Length; i++) {
                 sb.Append(input[i]).Append("\n");
        }
        return sb.ToString();
    }
	
	public static string PrefixZeros (int frames, int length){
		string s = frames.ToString ();
		string p = "";
		for (int i = 0; i<length-s.Length; i++)
			p += "0";
		return p + s;
	}

	static public void DestroyAllChildrenIn(GameObject obj,bool recurse){
		for(int i = obj.transform.childCount-1;i>=0;i--){
			if(recurse)DestroyAllChildrenIn(obj.transform.GetChild(i).gameObject,true);
			GameObject.Destroy(obj.transform.GetChild(i).gameObject);
		}
	}

	static public List<Transform> GetAllSubChildren(Transform obj, List<Transform> a = null){
		if (a == null)
			a = new List<Transform> ();
		if (obj.childCount == 0) {
			a.Add(obj);
			return a;
		}
		for(int i = 0;i< obj.childCount;i++){
			a = GetAllSubChildren(obj.GetChild(i),a);
		}
		return a;
	}

	static public Component MoveComponent(Component c, GameObject moveTo){
		Component b = moveTo.AddComponent(c.GetType());
		foreach (FieldInfo f in c.GetType().GetFields()){
			f.SetValue(b, f.GetValue(c));
		}
		return b;
	}

	public static void ZeroChildPosition (Transform transform){
		for(int i = 0; i<transform.childCount;i++){
			ZeroChildPosition(transform.GetChild(i));
		}
	}

	public static List<T> CopyList <T>(List<T> from){
		List<T> to = new List<T> ();
		foreach (T t in from)
			to.Add (t);
		return to;
	}

	public static List<T> RandomizeList<T> (List<T> list){
		List<T> a = new List<T>();
		while (list.Count!=0) {
			int i = Mathf.FloorToInt(Random.value*list.Count);
			a.Add(list[i]);
			list.RemoveAt(i);
		}
		list = null;
		return a;
	}

	public static System.Type GetType( string TypeName ){		
		var type = System.Type.GetType( TypeName );		
		if( type != null )
			return type;
		
		if( TypeName.Contains( "." ) ){
			var assemblyName = TypeName.Substring( 0, TypeName.IndexOf( '.' ) );
			var assembly = Assembly.Load( assemblyName );
			if( assembly == null )
				return null;
			type = assembly.GetType( TypeName );
			if( type != null )
				return type;
		}
		var currentAssembly = Assembly.GetExecutingAssembly();
		var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
		foreach( var assemblyName in referencedAssemblies ){
			var assembly = Assembly.Load( assemblyName );
			if( assembly != null ){
				type = assembly.GetType( TypeName );
				if( type != null )
					return type;
			}
		}
		return null;		
	}
	
	public delegate void FloatCallback(float value);
	public delegate void Callback();
	public static IEnumerator AnimationCoroutine(AnimationCurve curve, float time, FloatCallback callback, Callback onFinish = null){
		float t = time;
		while (t>0) {
			t=Mathf.Max(t-Time.deltaTime,0);
			callback(curve.Evaluate(1-t/time));
			yield return null;
		}
		if (onFinish != null)
			onFinish ();
	}

}
