using UnityEngine;
using System.Collections;
[System.Serializable]
public class CCReflectFloat : CCReflector<float>
{
	public bool isVector;
	public int index = 0;
	string order = "xyzw";
	string[] s;

	public CCReflectFloat(){
		types = new string[]{ typeof(float).ToString(), typeof(System.Int32).ToString(), typeof(Vector2).ToString(),typeof(Vector3).ToString(),typeof(Vector4).ToString()};
	}

	public override void ReloadField (){
		if (varName.IndexOf ("-") != -1) {
			string[] mark = new string[1];
			mark[0] = "-";
			s = varName.Split(mark, System.StringSplitOptions.None);
			isVector = true;
			index = order.IndexOf(s[1]);
		}
		base.ReloadField ();
	}

	public override object GetValue (){
		if (isMat) {
			if(isVector)return ((Material)obj).GetVector(_varName);
			return ((Material)obj).GetFloat(_varName);
		}
		return base.GetValue ();
	}
	
	public float GetFloat(){
		if (isVector) {
			string t = varName;
			_varName = varName = s[0];
			object v = base.GetValue();
			if(isMat)v=  GetValue();
			if(v.GetType() == typeof(Vector4)){
				Vector4 vec = (Vector4) v;
				return vec[index];
			}else if(v.GetType() == typeof(Vector3)){
				Vector3 vec = (Vector3) v;
				return vec[index];
			}else if(v.GetType() == typeof(Vector2)){
				Vector2 vec = (Vector2) v;
				return vec[index];
			}
			varName = _varName = t;
		}
		return (float)GetValue ();
	}

	public override void SetValue (object value){
		CheckVar ();
		if (!isVector) {
			if (isMat) {
				((Material)obj).SetFloat(varName,(float)value);
				return;
            }
            else
            {
                if (GetValue().GetType() == typeof(System.Int32))
                    base.SetValue(Mathf.FloorToInt((float)value));
                else
                    base.SetValue(value);
            }
				
		}else {
			string t = varName;
			_varName = varName = s[0];
			object v = GetValue();
			if(v.GetType() == typeof(Vector4)){
				Vector4 vec = (Vector4) v;
				vec[index] = (float)value;
				if(isMat)((Material)obj).SetVector(varName,vec);
				else base.SetValue(vec);
			}else if(v.GetType() == typeof(Vector3)){
				Vector3 vec = (Vector3) v;
				vec[index] = (float)value;
				base.SetValue(vec);
			}else if(v.GetType() == typeof(Vector2)){
				Vector2 vec = (Vector2) v;
				vec[index] = (float)value;
				base.SetValue(vec);
			}
			varName = _varName = t;
		}
	}
}

