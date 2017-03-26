using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour {

	public enum Emotion { HAPPY, DESIRE, ANGRY, SCARED, SAD, CONFUSED};

	protected ABaseUnitTemplate Template;

	public int energy {get; set;}

	public int maximumEnergy {get{return Template.maximumEnergy;}}

	public int energyPerCellMoved {get{return Template.energyPerCellMoved;}}

	public int energyPerHeightMovedDown {get{return Template.energyPerHeightMovedDown;}}

	public int energyPerHeightMovedUp {get{return Template.energyPerHeightMovedUp;}}

	public int maximumHeightPassableUp {get{return Template.maximumHeightPassableUp;}}

	public int maximumHeightPassableDown {get{return Template.maximumHeightPassableDown;}}

	public int maximumHealth {get{return Template.maximumHealth;}}

	private int health;

	public Emotion emotion { get{return activeOpinion != null ? activeOpinion.getEmotion(this) : Emotion.CONFUSED;}}

	private HexCell _hexCell;
	public HexCell hexCell {get{return _hexCell;} 
					set{if(_hexCell != null){_hexCell.Unit = null;}
						_hexCell = value;
						value.Unit = this;
						updatePosition();}}

    float height = 0f;
    List<HexCell> path;
    Vector3 vector {get{return hexCell.getPosition();}}
    float cellHeight {get{return hexCell.getHeight() + height;}}

    private IGoal goal;

    private IOpinion activeOpinion;

	public virtual void Make(HexCell initialCell, ABaseUnitTemplate template){
		hexCell = initialCell;
		Template = template;
	}

	public bool canContinue(){
		Debug.Log(path);
		return path != null && path.Count > 1 && path[path.Count - 2].Unit == null && (energy > 0 && hexCell != goal.getDestination());
	}

	private void updatePosition(){
		gameObject.transform.position = new Vector3(hexCell.fx, cellHeight, hexCell.fy); 
	}

	public void findPath(){
		if(goal.getDestination() != null && hexCell != goal.getDestination()){
			int distance;
			HexCell falseDestination = PathfindingUtil.findClosestOpenCell(this, goal.getDestination(), out distance, Template.getPathfindingConditional());
			if(falseDestination != null && distance < PathfindingUtil.hexGridDistance(hexCell, goal.getDestination())){
				findPath(falseDestination);
			}
			else{
				path = null;
			}
		}
	}

	/*
	* Works backwards from the true goal.getDestination() to find the first open cell*/

	public void findPath(HexCell destination){
		path = PathfindingUtil.findPath(this, hexCell, goal.getDestination(), Template.getPathfindingConditional(), Template.getPathfindingCalculator());
	}

	public void step(){
		if(canContinue()){
			int nextStepEnergyCost = Template.getPathfindingCalculator().calculate(this, path[path.Count - 2], hexCell);
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
		return hexCell != goal.getDestination() && !canContinue();
	}

	public void prepareForReaction(){
		energy = maximumEnergy;
		// Post-process buffs/debuffs
		activeOpinion = Template.getActiveOpinion(this);
		goal = activeOpinion.getGoal(this);
		findPath(goal.getDestination());
	}

	public void Update() {
		//transform.LookAt(Camera.main.transform.position, -Vector3.up);
		transform.LookAt(new Vector3(Camera.main.transform.position.x, 100f, Camera.main.transform.position.z));
		//transform.eulerAngles.x = 45.0f;
	}
}
