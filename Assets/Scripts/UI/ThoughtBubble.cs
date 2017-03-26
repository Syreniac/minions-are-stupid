using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour {

	TopLevelUI parent;
	Text childText;
	// Use this for initialization
	void Start () {
		parent = transform.parent.GetComponent<TopLevelUI>();
		childText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(parent.highlightedCell != null){
			if(parent.highlightedCell.Unit != null){
				transform.position = parent.highlightedCell.Unit.transform.position;
				transform.rotation = parent.highlightedCell.Unit.transform.rotation;

				childText.text = UIInterpreter.InterpretEmotion(parent.highlightedCell.Unit);

			}
			else{
				transform.position = new Vector3(99999f,99999f,99999f);
			}
		}
	}
}

