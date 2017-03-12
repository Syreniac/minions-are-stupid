using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitStats : BaseUnitStats {

	private static readonly PathfindingConditional pathfindingConditional;
	private static readonly PathfindingCalculator pathfindingCalculator;

	static TestUnitStats(){
		pathfindingConditional = new TestUnitConditional();
		pathfindingCalculator = new TestUnitCalculator();
	}

	public TestUnitStats(){
		maximumEnergy = 100;
		energyPerHeightMovedUp = 1;
		energyPerHeightMovedDown = 0;
		energyPerCellMoved = 20;
		maximumHeightPassableUp = 10;
		maximumHeightPassableDown = 10;

	}

	public override PathfindingConditional getPathfindingConditional(){
		return pathfindingConditional;
	}

	public override PathfindingCalculator getPathfindingCalculator(){
		return pathfindingCalculator;
	}
}

public class TestUnitConditional : PathfindingConditional{

	public override bool checkCondition(BaseUnit unit, HexCell into, HexCell from){
		if(into.Unit == null){
			if(into.getHeight() - from.getHeight() > 0){
				// Uphill
				return unit.maximumHeightPassableUp > into.getHeight() - from.getHeight();
			}
			return into.getHeight() - from.getHeight() > -unit.maximumHeightPassableDown;
		}
		return false;

	}

}

public class TestUnitCalculator : PathfindingCalculator{
	public override int calculateCost(BaseUnit unit, HexCell into, HexCell from){
		if(into.getHeight() - from.getHeight() > 0){
			// Uphill
			return unit.energyPerCellMoved + ((into.getHeight() - from.getHeight()) * unit.energyPerHeightMovedUp);
		}
		return unit.energyPerCellMoved + ((from.getHeight() - into.getHeight()) * unit.energyPerHeightMovedDown);
	}
}
