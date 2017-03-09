using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour {

	private HexCell _hexCell;
	public HexCell hexCell {get{return _hexCell;} 
					set{if(_hexCell != null){_hexCell.Unit = null;}
						_hexCell = value;
						value.Unit = this;
						updatePosition();}}

    float height = 10f;
    List<HexCell> path;
    Vector3 vector {get{return hexCell.getPosition();}}
    float cellHeight {get{return hexCell.getHeight() + height;}}
    public HexCell destination;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Make(HexCell initialCell){
		hexCell = initialCell;
	}

	public bool canContinue(){
		return path != null && path.Count > 1 && path[path.Count - 2].Unit == null;
	}

	private void updatePosition(){
		gameObject.transform.position = new Vector3(hexCell.fx, cellHeight, hexCell.fy); 
	}

	public void debugDrawPath(){
	}

	public void findPath(){
		if(destination != null && hexCell != destination){
			int distance;
			HexCell falseDestination = PathfindingUtil.findClosestOpenCell_TerrainAndUnits(destination, out distance);
			if(falseDestination != null && distance < PathfindingUtil.hexGridDistance(hexCell, destination)){
				findPath(falseDestination);
			}
			else{
				path = null;
			}
		}
	}

	/*
	* Works backwards from the true destination to find the first open cell*/

	public void findPath(HexCell destination){
		path = PathfindingUtil.findPath_Units(hexCell, destination);
	}

	public void step(){
		if(canContinue()){
			hexCell = path[path.Count - 2];
			path.Remove(path[path.Count - 2]);	
		}
	}
}
