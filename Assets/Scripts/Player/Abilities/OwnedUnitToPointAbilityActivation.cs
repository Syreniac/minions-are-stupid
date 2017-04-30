using UnityEngine;

public class OwnedUnitToPointAbilityActivation : AAbilityActivation{

	public HexCell Target {private set; get;}

	public BaseUnit Unit {private set; get;}

	public OwnedUnitToPointAbilityActivation(IAbilityTemplate template) : base(template){
		Debug.Log("Making activation");
	}

	public override bool isReady(){
		return Unit != null && Target != null;
	}

	public override void clickedCell(HexCell cell, KeyCode key){
		if(Unit == null){
			if(cell != null && cell.Unit != null && cell.Unit.OwnedBy(Player)){
				Unit = cell.Unit;
			}
		}
		else{
			Target = cell;
		}
	}
}