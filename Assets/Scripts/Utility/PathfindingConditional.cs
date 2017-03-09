using System.Collections;
using System.Collections.Generic;

abstract public class PathfindingConditional {

	public PathfindingConditional next {get; set;}

	public bool check(HexCell hexCell){
		if(next != null){
			return checkCondition(hexCell) && next.check(hexCell);
		}
		return checkCondition(hexCell);
	}

	public abstract bool checkCondition(HexCell cell);

}

public class BlankPathfindingConditional : PathfindingConditional{

	public override bool checkCondition(HexCell hexCell){
		return true;
	}

}

public class UnitPathfindingConditional : PathfindingConditional{

	public override bool checkCondition(HexCell hexCell){
		return hexCell.Unit == null;
	}

}

public class TerrainPathfindingConditional : PathfindingConditional{

	public override bool checkCondition(HexCell hexCell){
		return !hexCell.immovable;
	}

} 