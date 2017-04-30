using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBank {

	private int currentMana;

	private int maxMana;

	public bool canAfford(int manaCost){
		return manaCost <= currentMana;
	}

	public void subtractMana(int manaCost){
		currentMana -= manaCost;
	}

}
