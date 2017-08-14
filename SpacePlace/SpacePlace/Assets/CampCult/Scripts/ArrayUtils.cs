using UnityEngine;
using System.Collections;

public class ArrayUtils 
{
	public static int[,,] Create3D(int fill,int x, int y, int z){
		int[,,] a = new int[x,y,z];
		for(int i = 0; i<x;i++){
			for(int j = 0; j<y;j++){
				for(int k = 0; k<z;k++){
					a[i,j,k] = fill;
				}
			}
		}
		return a;
	}
	public static int[,] Create2D(int fill,int x, int y){
		int[,] a = new int[x,y];
		for(int i = 0; i<x;i++){
			for(int j = 0; j<y;j++){
				a[i,j] = fill;
			}
		}
		return a;
	}

	public static int[,,] Clone3D(int[,,] a){
		return a.Clone() as int[,,];
	}

	public static int[,,] Merge3D(int[,,] parent, int[,,] child, int x, int y, int z){
		for(int i = 0; i<child.GetLength(0);i++){
			for(int j = 0; j<child.GetLength(1);j++){
				for(int k = 0; k<child.GetLength(2);k++){
					if( (i+x) < parent.GetLength(0) && (j+y)< parent.GetLength(1) &&(k+z)<parent.GetLength(2)){
						//Debug.Log(parent.GetLength(0)+"  "+(i+x)+" - "+parent.GetLength(1)+"  "+(j+y)+" - "+parent.GetLength(2)+"  "+(k+z));
						parent[i+x,j+y,k+z] = child[i,j,k];
					}
				}
			}
		}
		return parent;
	}
	
	public static int[,,] SwapValue3D(int[,,] a, int oldValue, int newValue){
		for(int i = 0; i<a.GetLength(0);i++){
			for(int j = 0; j<a.GetLength(1);j++){
				for(int k = 0; k<a.GetLength(2);k++){
					if(a[i,j,k]==oldValue) a[i,j,k]=newValue;
				}
			}
		}
		return a;
	}

	public static int[,,] Flood3D(int[,,] a, int x, int y, int z, int oldValue, int newValue){
		if(x>-1 && x<a.GetLength(0) && y>-1 && y<a.GetLength(1) && z>-1 && z<a.GetLength(2)){
			if(a[x,y,z] == oldValue){
				a[x,y,z] = newValue;
				Flood3D(a,x-1,y,z,oldValue,newValue);
				Flood3D(a,x+1,y,z,oldValue,newValue);
				Flood3D(a,x,y-1,z,oldValue,newValue);
				Flood3D(a,x,y+1,z,oldValue,newValue);
				Flood3D(a,x,y,z-1,oldValue,newValue);
				Flood3D(a,x,y,z+1,oldValue,newValue);
			}
		}
		return a;
	}

	public static bool hasValue3D(int[,,] a, int value) {
		for (int i = 0; i < a.GetLength(0); i++) {
			for(int j = 0; j<a.GetLength(1);j++){
				for(int k = 0; k<a.GetLength(2);k++){
					if(a[i,j,k]==value)return true;
				}
			}
		}
		return false;
	}

	public static T getRandom<T>(T[] a){
		return a [Mathf.FloorToInt (Random.value * a.Length)];
	}
	
}

