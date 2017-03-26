using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpinion1 : IOpinion {
	// Unit wants to sit on mountains
	public BaseUnit.Emotion getEmotion(BaseUnit unit){
		if(unit.hexCell.getHeight() > 40){
			return BaseUnit.Emotion.HAPPY;
		}
		return BaseUnit.Emotion.SAD;
	}
	public IGoal getGoal(BaseUnit unit){
		if(unit.hexCell.getHeight() > 40){
			return new TileGoal(unit.hexCell);
		}
		List<HexCell> toTest = new List<HexCell>();
		unit.hexCell.addNeighbourCells(toTest);
		int iterations = 0;
		int k = 1;
		while(iterations < 5){
			int j = k;
			k = toTest.Count;
			while(j < k){
				if(toTest[j].getHeight() > 40 && toTest[j].Unit == null){
					return new TileGoal(toTest[j]);
				}
				else{
					toTest[j].addNeighbourCells(toTest);
				}
				j++;
			}
			iterations++;
		}
		return new TileGoal(unit.hexCell);
	}
	public int getValue(BaseUnit unit){
		return 9999;
	}

}
