using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseUnit : MonoBehaviour {

	protected BaseUnitStats unitStats;

	public int energy {get; set;}

	public int maximumEnergy {get{return unitStats.maximumEnergy;}}

	public int energyPerCellMoved {get{return unitStats.energyPerCellMoved;}}

	public int energyPerHeightMovedDown {get{return unitStats.energyPerHeightMovedDown;}}

	public int energyPerHeightMovedUp {get{return unitStats.energyPerHeightMovedUp;}}

	public int maximumHeightPassableUp {get{return unitStats.maximumHeightPassableUp;}}

	public int maximumHeightPassableDown {get{return unitStats.maximumHeightPassableDown;}}

	public int maximumHealth {get{return unitStats.maximumHealth;}}

	private int health;

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

	public virtual void Make(HexCell initialCell){
		hexCell = initialCell;
	}

	public bool canContinue(){
		return path != null && path.Count > 1 && path[path.Count - 2].Unit == null && (energy > 0 && hexCell != destination);
	}

	private void updatePosition(){
		gameObject.transform.position = new Vector3(hexCell.fx, cellHeight, hexCell.fy); 
	}

	public void findPath(){
		if(destination != null && hexCell != destination){
			int distance;
			HexCell falseDestination = PathfindingUtil.findClosestOpenCell(this, destination, out distance, unitStats.getPathfindingConditional());
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
		path = PathfindingUtil.findPath(this, hexCell, destination, unitStats.getPathfindingConditional(), unitStats.getPathfindingCalculator());
	}

	public void step(){
		if(canContinue()){
			int nextStepEnergyCost = unitStats.getPathfindingCalculator().calculate(this, path[path.Count - 2], hexCell);
			if(nextStepEnergyCost <= energy){
				hexCell = path[path.Count - 2];
				path.Remove(path[path.Count - 2]);
				energy -= nextStepEnergyCost;
			}
			else{
				energy = 0;
			}	
		}
	}

	public bool nextStepIsConflicted(){
		return hexCell != destination && !canContinue();
	}

	public void prepareForReaction(){
		energy = maximumEnergy;
		// Post-process buffs/debuffs
	}
}
