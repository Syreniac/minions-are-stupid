using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour {

	public bool Active {get; set;}

	HexGrid hexGrid;

	private float nextActionTime;

	int steps;

	// Use this for initialization
	void Start () {
		Active = false;
		hexGrid = GetComponent<HexGrid>();
	}

	public void begin(){
		nextActionTime = 0f;
		foreach(BaseUnit unit in hexGrid.Units){
			unit.prepareForReaction();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Active){
			Debug.Log("reaction controller run");
			if(Time.time  > nextActionTime){
				foreach(BaseUnit unit in hexGrid.Units){
					if(unit.nextStepIsConflicted()){
						unit.findPath();
					}
				}
				foreach(BaseUnit unit in hexGrid.Units){
					unit.step();
				}
				nextActionTime = Time.time + 0.1f;
				steps--;
				if(!hexGrid.canContinue()){
					Active = false;
				}
				foreach(HexChunk chunk in hexGrid.chunks){
					chunk.RerenderCells();
				}
			}
		}
	}


}
