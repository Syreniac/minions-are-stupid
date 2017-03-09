using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMoveCommand : ICommand {

	public List<TestUnit> Units {get; private set;}

	public HexCell Destination {get; private set;}

	public GroupMoveCommand(){
		Units = new List<TestUnit>();
		Destination = null;
	}

	public void Execute(){
		foreach(TestUnit testUnit in Units){
			testUnit.destination = Destination;
		}
	}

	public int getPriority(){
		return 1;
	}

	public bool isReady(){
		return Units.Count > 0 && Destination != null;
	}

	public void clickedCell(HexCell cell){
		Debug.Log(Input.GetKey("left shift"));
		if((Input.GetKey("left shift") || Units.Count == 0) && !Units.Contains(cell.Unit)){
			if(cell.Unit != null){
				Units.Add(cell.Unit);
			}
		}
		else{
			Destination = cell;
		}
	}
}
