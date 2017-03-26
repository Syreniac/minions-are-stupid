using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOpinion{
	BaseUnit.Emotion getEmotion(BaseUnit unit);
	IGoal getGoal(BaseUnit unit);
	int getValue(BaseUnit unit);
}