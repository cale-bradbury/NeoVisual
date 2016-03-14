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
        t.transform.localScale = Vector3.one;
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
            g.transform.localPosition = new Vector3(0,g.transform.localPosition.y,0);
            g.transform.eulerAngles = new Vector3(270, Random.value * 360,0);
            g.transform.localScale = Vector3.one;
            b[i] = g;
        }
	}

    void OnDestroy(){
        for (int i = 0; i < b.Length; i++)
            Destroy(b[i]);
    }
}
