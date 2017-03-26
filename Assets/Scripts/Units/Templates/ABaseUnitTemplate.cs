using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ABaseUnitTemplate{

	protected static readonly GameObject BASE_UNIT_PREFAB = (GameObject)Resources.Load("BaseUnit");

	public List<BaseUnit> units {get; protected set;}

	public int maximumEnergy {get; protected set;}

	public int energyPerCellMoved {get;protected set;}

	public int energyPerHeightMovedDown {get;protected set;}

	public int energyPerHeightMovedUp {get;protected set;}

	public int maximumHeightPassableUp {get;protected set;}

	public int maximumHeightPassableDown {get;protected set;}

	public int maximumHealth {get;protected set;}

	public abstract PathfindingConditional getPathfindingConditional();

	public abstract PathfindingCalculator getPathfindingCalculator();

	public abstract BaseUnit make(HexCell cell);

	public abstract IOpinion getActiveOpinion(BaseUnit unit);
}
