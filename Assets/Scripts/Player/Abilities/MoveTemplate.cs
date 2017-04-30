using UnityEngine;

public class MoveTemplate : IAbilityTemplate{

	public AAbilityActivation makeAbilityActivation(HexCell cell, KeyCode keyCode){
		AAbilityActivation activation = new OwnedUnitToPointAbilityActivation(this);
		activation.clickedCell(cell, keyCode);
		return activation;
	}

	public void execute(AAbilityActivation activation){
		Debug.Log("Executing");
		OwnedUnitToPointAbilityActivation trueActivation = (OwnedUnitToPointAbilityActivation) activation;
		trueActivation.Unit.orderedGoal = new TileGoal(trueActivation.Target);
	}

	public int calculateManaCost(AAbilityActivation activation, HexCell cell){
		return 0;
	}

}