using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGoal : IGoal {

	private HexCell HexCell;

	public TileGoal(HexCell hexCell){
		HexCell = hexCell;
	}

	public HexCell getDestination(){
		return HexCell;
	}
	public HexCell getCell(){
		return HexCell;
	}
	public bool shouldAttack(){
		return false;
	}

}
