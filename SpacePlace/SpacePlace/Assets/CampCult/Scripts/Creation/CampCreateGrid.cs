using UnityEngine;
using System.Collections;

public class CampCreateGrid : CampCreateGroup {
	private int _rows;
	private int _columns;
	public int rows = 4;
	public int columns = 4;
	private Vector3 _offsetPerColumn;
	public Vector3 offsetPerColumn;
	private Vector3 _offsetPerRow;
	public Vector3 offsetPerRow;

	override public Vector3 place(int i){
		return (offsetPerRow*Mathf.Ceil(i/columns))+(offsetPerColumn*(i%columns));
	}
	
	protected override void Start(){
		count = rows*columns;
		base.Start();
		_offsetPerRow = offsetPerRow;
		_offsetPerColumn = offsetPerColumn;
		_rows = rows;
		_columns = columns;
	}
	
	protected override void Update(){
		base.Update();
		if(_offsetPerColumn!=offsetPerColumn){
			_offsetPerColumn = offsetPerColumn;
			refresh();
		}
		if(_offsetPerRow!=offsetPerRow){
			_offsetPerRow = offsetPerRow;
			refresh();
		}
		if(_rows!=rows){
			_rows = rows;
			count = rows*columns;
			refresh();
		}
		if(_columns!=columns){
			_columns = columns;
			count = rows*columns;
			refresh();
		}
	}
}
