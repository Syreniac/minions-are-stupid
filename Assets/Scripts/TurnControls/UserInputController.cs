using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputController : MonoBehaviour {

	public bool Active {get; set;}

	private ExecutionController executionController;
	private HexGrid hexGrid;

	private ICommand command;

	void Awake() {
		executionController = GetComponent<ExecutionController>();
		hexGrid = GetComponent<HexGrid>();
		Active = false;
		command = null;
	}

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
		if(Active) {
			if(Input.GetKeyDown("space")){
				Active = false;
			}
			else if(command != null && command.isReady()){
				executionController.postCommand(command);
				Active = false;
				command = null;
			} else {
				processCommand();
			}
		}
	}

	void processCommand(){
		Nullable<KeyCode> keyCode = shouldMakeNewCommand();
		if(keyCode != null){
			makeNewCommand(keyCode);
		}
		keyCode = shouldClickGrid();
		if(keyCode != null){
			HexCell clickedCell = hexGrid.projectRay();
			if(clickedCell != null){
				Debug.Log("clicked on cell");
				if(command != null){
					command.clickedCell(clickedCell, (KeyCode) keyCode);
				}
			}
		}
	}

	Nullable<KeyCode> shouldClickGrid(){
		if(Input.GetMouseButtonDown(0)){
			return KeyCode.Mouse0;
		}
		if(Input.GetMouseButtonDown(1)){
			return KeyCode.Mouse1;
		}
		return null;
	}

	void makeNewCommand(Nullable<KeyCode> keyCode){
		if(keyCode == KeyCode.Mouse0){
			Debug.Log("making new command");
			command = new GroupMoveCommand();
		}
	}

	Nullable<KeyCode> shouldMakeNewCommand(){
		if(Input.GetMouseButtonDown(0) && !(command is GroupMoveCommand)){
			return KeyCode.Mouse0;
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			return KeyCode.Q;
		}
		return null;
	}

	public void Button(){
		Debug.Log("Button");
		if(Active){
			Active = false;
		}
	}

}
