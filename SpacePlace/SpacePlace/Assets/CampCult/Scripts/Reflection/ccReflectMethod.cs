using UnityEngine;
using System.Reflection;

[System.Serializable]
public class ccReflectMethod {

    public Object obj;
    public string methodName;
    public MethodInfo method;

    public void Invoke()
    {
        if (method == null)
        {
            method = obj.GetType().GetMethod(methodName);
            if (method == null)
                return;
        }
        method.Invoke(obj, null);
    }

}
