using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{
	private static float mul = 1;

	private static Vector3[] childDirections = {
		Vector3.up*mul,
		Vector3.right*mul,
		Vector3.left*mul,
		Vector3.forward*mul,
		Vector3.back*mul,
		Vector3.down*mul
	};
	
	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f),
		Quaternion.Euler(0f,-90,0f)
	};

	public Mesh mesh;
	public Material material;
	public int maxDepth;	
	private int depth;
	public float childScale;

	void Update(){
		if(!GetComponent<Renderer>().isVisible)return;
		Vector3 q =  this.transform.localEulerAngles;
		q.y+=.1f*depth;
		transform.localEulerAngles = q;
	}

	private void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
		if (depth < maxDepth) {
			StartCoroutine(CreateChildren());
		}
		if(depth==0){
			GetComponent<Renderer>().enabled = false;
		}
	}
	
	private IEnumerator CreateChildren () {
		for (int i = 0; i < childDirections.Length; i++) {
			yield return new WaitForSeconds(0.1f);
				new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
		}
	}
	
	private void Initialize (Fractal parent, int childIndex) {
		mesh = parent.mesh;
		material = parent.material;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}

}

