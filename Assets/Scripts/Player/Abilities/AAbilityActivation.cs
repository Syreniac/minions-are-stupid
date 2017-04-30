using UnityEngine;

abstract public class AAbilityActivation{

	protected readonly IAbilityTemplate templatedFrom;

	protected int manaCost;

	public Player Player {get; set;}

	public AAbilityActivation(IAbilityTemplate templatedFrom){
		this.templatedFrom = templatedFrom;
	}

	public void Execute(){
		templatedFrom.execute(this);
	}

	public int calculateManaCost(HexCell cell){
		return templatedFrom.calculateManaCost(this, cell);
	}

	abstract public bool isReady();

	abstract public void clickedCell(HexCell cell, KeyCode key);

}