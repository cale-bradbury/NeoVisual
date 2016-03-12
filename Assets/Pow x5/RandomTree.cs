using UnityEngine;
using System.Collections;

public class RandomTree : MonoBehaviour {

    public GameObject trunk;
    public GameObject[] branches;
    public int minBranches;
    public int maxBranches;
    GameObject[] b;
    GameObject t;

	// Use this for initialization
	void OnEnable () {
        Create();
        t = Instantiate<GameObject>(trunk);
        t.transform.parent = transform;
        t.transform.localPosition = Vector3.zero;
	}
    int c = 0;
    void Update()
    {
        c++;
        if(c%100==0)
            Create();
    }
	
	// Update is called once per frame
	void Create () {
        if (b != null)
            OnDestroy();
        int num = Random.Range(minBranches, maxBranches);
        b = new GameObject[num];
        for(int i = 0; i< num; i++)
        {
            GameObject g = Instantiate(branches[Mathf.FloorToInt(Random.value * branches.Length)]);
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            g.transform.localEulerAngles = Vector3.up * Random.value * 360;
            b[i] = g;
        }
	}

    void OnDestroy(){
        for (int i = 0; i < b.Length; i++)
            Destroy(b[i]);
    }
}
