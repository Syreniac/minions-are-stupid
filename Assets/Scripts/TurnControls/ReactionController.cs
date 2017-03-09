using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour {

	public bool Active {get; set;}

	HexGrid hexGrid;

	private float nextActionTime;

	int steps;

	readonly int MAXSTEPS = 1;

	// Use this for initialization
	void Start () {
		Active = false;
		hexGrid = GetComponent<HexGrid>();
	}

	public void begin(){
		nextActionTime = 0f;
		steps = MAXSTEPS;
	}
	
	// Update is called once per frame
	void Update () {
		if(Active){
			Debug.Log("reaction controller run");
			if(Time.time  > nextActionTime){
				foreach(TestUnit testUnit in hexGrid.testUnits){
					testUnit.findPath();
				}
				foreach(TestUnit testUnit in hexGrid.testUnits){
					testUnit.step();
				}
				nextActionTime = Time.time + 0.5f;
				steps--;
				if(steps == 0 || !hexGrid.canContinue()){
					Active = false;
				}
				foreach(HexChunk chunk in hexGrid.chunks){
					chunk.RerenderCells();
				}
			}
		}
	}


}
