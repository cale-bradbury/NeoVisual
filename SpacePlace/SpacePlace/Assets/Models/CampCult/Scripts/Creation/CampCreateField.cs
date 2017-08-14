using UnityEngine;
using System.Collections;

public class CampCreateField : CampCreateGroup {

	public Vector3 distance;
	private Vector3 _distance;

	public bool center = true;
	private bool _center;
    public bool containWithinBounds = false;

    override protected void Update()
    {
        base.Update();
        if (_distance != distance)
        {
            _distance = distance;
            refresh();
        }
        if (_center != center)
        {
            _center = center;
            refresh();
        }
        if (!containWithinBounds)
            return;
        Vector3 v;
        for (int i = 0; i < all.Count; i++)
        {
            v = all[i].transform.localPosition;
            if (center)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (v[j] < -distance[j]*.5f)
                        v[j] += distance[j]*.5f;
                    else if (v[j] > distance[j]*.5f)
                        v[j] -= distance[j]*.5f;
                }
            }
            else{ 
                for (int j = 0; j < 3; j++)
                {
                    if (v[j] < 0)
                        v[j] += distance[j];
                    else if (v[j] > distance[j])
                        v[j] -= distance[j];
                }
            }

            all[i].transform.localPosition = v;
        }
    }

	public override Vector3 place (int i)
	{
		Vector3 p  = new Vector3(Random.Range(0,distance.x),Random.Range(0,distance.y),Random.Range(0,distance.z));
		if(center){
			p -= distance*.5f;
		}
		return p;
	}

	// Use this for initialization
	new void Start () {
		base.Start();
		_distance = distance;
		_center = center;
	}
	
}
