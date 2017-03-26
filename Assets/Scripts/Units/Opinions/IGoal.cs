using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoal{
	HexCell getDestination();
	HexCell getCell();
	bool shouldAttack();
}
