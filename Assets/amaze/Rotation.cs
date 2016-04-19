using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

	public float direction = 1.0f;

	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * direction);
	}
}
