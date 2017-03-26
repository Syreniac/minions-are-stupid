using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinUnitTemplate : ABaseUnitTemplate {

	private PathfindingConditional pathfindingConditional;
	private PathfindingCalculator pathfindingCalculator;

	private List<IOpinion> opinions;

	public override BaseUnit make(HexCell cell){
		GameObject go = GameObject.Instantiate(ABaseUnitTemplate.BASE_UNIT_PREFAB);
		BaseUnit unit = go.GetComponent<BaseUnit>();
		unit.Make(cell, this);
		return unit;
	}

	public GoblinUnitTemplate(){
		pathfindingConditional = new GoblinUnitConditional();
		pathfindingCalculator = new GoblinUnitCalculator();
		maximumEnergy = 100;
		energyPerHeightMovedUp = 1;
		energyPerHeightMovedDown = 0;
		energyPerCellMoved = 20;
		maximumHeightPassableUp = 10;
		maximumHeightPassableDown = 10;
		opinions = new List<IOpinion>();
		opinions.Add(new TestOpinion1());
	}

	public override PathfindingConditional getPathfindingConditional(){
		return pathfindingConditional;
	}

	public override PathfindingCalculator getPathfindingCalculator(){
		return pathfindingCalculator;
	}

	public override IOpinion getActiveOpinion(BaseUnit unit){
		IOpinion returnValue = opinions[0];
		foreach(IOpinion opinion in opinions){
			if(opinion.getValue(unit) > returnValue.getValue(unit)){
				returnValue = opinion;
			}
		}
		return returnValue;
	}
}

public class GoblinUnitConditional : PathfindingConditional{

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

public class GoblinUnitCalculator : PathfindingCalculator{
	public override int calculateCost(BaseUnit unit, HexCell into, HexCell from){
		if(into.getHeight() - from.getHeight() > 0){
			// Uphill
			return unit.energyPerCellMoved + ((into.getHeight() - from.getHeight()) * unit.energyPerHeightMovedUp);
		}
		return unit.energyPerCellMoved + ((from.getHeight() - into.getHeight()) * unit.energyPerHeightMovedDown);
	}
}