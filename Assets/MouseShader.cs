using UnityEngine;
using System.Collections;

public class MouseShader : MonoBehaviour {

    Vector4 mouse = Vector4.zero;
    public float smoothing = .9f;
	
    void OnEnable()
    {
        mouse.x = Input.mousePosition.x / Screen.width;
        mouse.y = Input.mousePosition.y / Screen.height;
    }

	void Update () {
        mouse = Vector4.Lerp(mouse, new Vector4(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height,
            Input.GetMouseButton(0) ? 1 : 0,
            0), smoothing);
        Shader.SetGlobalVector("_Mouse", mouse);
	}
}
