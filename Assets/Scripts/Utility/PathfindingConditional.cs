using System.Collections;
using System.Collections.Generic;

abstract public class PathfindingConditional {

	public PathfindingConditional next {get; set;}

	public bool check(BaseUnit unit, HexCell into, HexCell from){
		if(next != null){
			return checkCondition(unit, into, from) && next.check(unit, into, from);
		}
		return checkCondition(unit, into, from);
	}

	public abstract bool checkCondition(BaseUnit unit, HexCell into, HexCell from);

}

abstract public class PathfindingCalculator{
	public PathfindingCalculator next {get;set;}

	public int calculate(BaseUnit unit, HexCell into, HexCell from){
		if(next != null){
			return calculateCost(unit, into, from) + next.calculate(unit, into, from);
		}
		return calculateCost(unit, into, from);
	}

	/* Returned values less than zero will cause the unit to gain energy from moving rather than
	losing it!*/
	public abstract int calculateCost(BaseUnit unit, HexCell into, HexCell from);
}

public class BlankPathfindingConditional : PathfindingConditional{
	public override bool checkCondition(BaseUnit unit, HexCell into, HexCell from){
		return true;
	}
}

public class BlankPathfindingCalculator : PathfindingCalculator{
	public override int calculateCost(BaseUnit unit, HexCell into, HexCell from){
		return 0;
	}
}

