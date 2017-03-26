using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIInterpreter {

	public static string InterpretEmotion(BaseUnit unit) {
		BaseUnit.Emotion emo = unit.emotion; 
		switch(emo) {
			case BaseUnit.Emotion.HAPPY:
				return UIConstants.HAPPY;
			case BaseUnit.Emotion.DESIRE:
				return UIConstants.DESIRE;
			case BaseUnit.Emotion.ANGRY:
				return UIConstants.ANGRY;
			case BaseUnit.Emotion.SCARED:
				return UIConstants.SCARED;
			case BaseUnit.Emotion.SAD:
				return UIConstants.SAD;
			default:
				return "???";
		}
	}
}
