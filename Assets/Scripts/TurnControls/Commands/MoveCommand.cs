using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand{

	public TestUnit Unit {get; private set;}

	public HexCell Destination {get; private set;}

	public MoveCommand(){
		Unit = null;
		Destination = null;
	}

	public void Execute(){
		Unit.destination = Destination;
	}

	public int getPriority(){
		return 1;
	}

	public bool isReady(){
		return Unit != null && Destination != null;
	}

	public void clickedCell(HexCell cell){
		if(Unit == null){
			Unit = cell.Unit;
		}
		else{
			Destination = cell;
		}
	}
}
