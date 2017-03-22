using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour {

	TopLevelUI parent;

	// Use this for initialization
	void Start () {
		parent = transform.parent.GetComponent<TopLevelUI>();
	}
	
	// Update is called once per frame
	void Update () {
		if(parent.highlightedCell != null){
			if(parent.highlightedCell.Unit != null){
				transform.position = parent.highlightedCell.Unit.transform.position;
				transform.rotation = parent.highlightedCell.Unit.transform.rotation;

			}
			else{
				transform.position = new Vector3(99999f,99999f,99999f);
			}
		}
	}
}
