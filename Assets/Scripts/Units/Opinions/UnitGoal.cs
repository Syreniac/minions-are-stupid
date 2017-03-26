using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGoal : IGoal {

	private BaseUnit unit;

	public UnitGoal(BaseUnit unit){
		this.unit = unit;
	}

	public HexCell getDestination(){
		return unit.hexCell;
	}
	public HexCell getCell(){
		return unit.hexCell;
	}
	public bool shouldAttack(){
		return false;
	}
}
