using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMoveCommand : ICommand {

	public List<BaseUnit> Units {get; private set;}

	public HexCell Destination {get; private set;}

	public GroupMoveCommand(){
		Units = new List<BaseUnit>();
		Destination = null;
	}

	public void Execute(){
		foreach(BaseUnit unit in Units){
			unit.destination = Destination;
		}
	}

	public int getPriority(){
		return 1;
	}

	public bool isReady(){
		return Units.Count > 0 && Destination != null;
	}

	public void clickedCell(HexCell cell, KeyCode key){
		if(key == KeyCode.Mouse0 && cell.Unit != null && !Units.Contains(cell.Unit)){
			Units.Add(cell.Unit);	
		}
		else if(key == KeyCode.Mouse1){
			Destination = cell;
		}
	}
}
