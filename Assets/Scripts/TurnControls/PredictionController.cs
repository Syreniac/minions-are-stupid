using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionController : MonoBehaviour{

	HexGrid grid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startTurn(){
		grid.startTurn();
		foreach(Player player in grid.getPlayers()){
			player.startTurn();
		}
	}

}
