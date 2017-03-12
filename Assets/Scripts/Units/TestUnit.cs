using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : BaseUnit {

	public override void Make(HexCell initialCell){
		base.Make(initialCell);
		unitStats = new TestUnitStats();
	}

}
