using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLevelUI : MonoBehaviour {

	HexGrid grid;
	HexChunk highlightedChunk;
	public HexCell highlightedCell {get; private set;}

	// Use this for initialization
	void Start () {
		grid = GetComponent<HexGrid>();
	}
	
	void Update () {
		HighlightHoveredCell();
	}	

	void HighlightHoveredCell(){
	    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
	    if(!screenRect.Contains(Input.mousePosition)){
	    	// Resets
	    	if(highlightedCell != null){
	    		highlightedCell.highlighted = false;
	    		highlightedCell = null;
	    		highlightedChunk.RerenderCells();
	    		highlightedChunk = null;
	    	}
	    }
	    else{
	    	HexCell newCell;
	    	HexChunk newChunk;
	    	newCell = grid.projectRay(out newChunk);
	    	if(newCell != highlightedCell){
	    		if(highlightedCell != null){
	    			highlightedCell.highlighted = false;
	    			highlightedChunk.RerenderCells();
	    		}
	    		if(newCell != null){
	    			newCell.highlighted = true;
	    			newChunk.RerenderCells();
	    		}
	    		highlightedCell = newCell;
	    		highlightedChunk = newChunk;
	    	}
	    }
	}
}
