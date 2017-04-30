using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityTemplate {

	AAbilityActivation makeAbilityActivation(HexCell cell, KeyCode keyCode);

	void execute(AAbilityActivation activation);

	int calculateManaCost(AAbilityActivation activation, HexCell cell);

}