using UnityEngine;
using System.Collections;

public class CampCreateRing : CampCreateGroup {

	public enum Direction{
		XY,
		XZ,
		YZ
	}

	public Direction direction;
	private Direction _direction;

	public float radius;
	private float _radius;

    public float phase;
    private float _phase;

    public float arc;
    private float _arc;

    public override Vector3 place (int i){
		float a = (((float)i/(count-1)+phase/(count-1))*arc) *Mathf.PI*2;
		if(direction==Direction.XY)
			return new Vector3(Mathf.Cos(a)*radius,Mathf.Sin(a)*radius,0);
		else if(direction==Direction.XZ)
			return new Vector3(Mathf.Cos(a)*radius,0,Mathf.Sin(a)*radius);
		return new Vector3(0,Mathf.Cos(a)*radius,Mathf.Sin(a)*radius);
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		_radius = radius;
		_phase = phase;
		_direction = direction;
        _arc = arc;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		bool r = false;
		if(_radius!=radius){
			_radius = radius;
			r = true;
		}else if(_phase!=phase){
			_phase = phase;
			r = true;
        }else if (_direction != direction)
        {
            _direction = direction;
            r = true;
        }else if (_arc != arc)
        {
            _arc = arc;
            r = true;
        }

        if (r)
			refresh ();
	}

}
