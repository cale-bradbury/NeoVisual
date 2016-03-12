using UnityEngine;
using System.Collections;

public class RandomTree : ccEventBase {

    public GameObject trunk;
    public GameObject[] branches;
    public int minBranches;
    public int maxBranches;
    GameObject[] b;
    GameObject t;

    // Use this for initialization
    protected override void OnEnable()
    {
        base.OnEnable();
        Create();
    }

    void Start()
    {
        t = Instantiate<GameObject>(trunk);
        t.transform.parent = transform;
        t.transform.localPosition = Vector3.zero;
    }

    protected override void OnEvent()
    {
        base.OnEvent();
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
