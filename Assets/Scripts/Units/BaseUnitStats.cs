using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseUnitStats {

	public int maximumEnergy {get; protected set;}

	public int energyPerCellMoved {get;protected set;}

	public int energyPerHeightMovedDown {get;protected set;}

	public int energyPerHeightMovedUp {get;protected set;}

	public int maximumHeightPassableUp {get;protected set;}

	public int maximumHeightPassableDown {get;protected set;}

	public int maximumHealth {get;protected set;}

	public void prepareForReaction(BaseUnit unit){
		unit.energy = maximumEnergy;
	}

	public abstract PathfindingConditional getPathfindingConditional();

	public abstract PathfindingCalculator getPathfindingCalculator();
}
