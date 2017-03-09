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
		command = new GroupMoveCommand();
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
			else if(command.isReady()){
				executionController.postCommand(command);
				Active = false;
				command = new GroupMoveCommand();
			}
			else{
				if(Input.GetMouseButtonDown(0)){
					HexCell clickedCell = hexGrid.projectRay();
					if(clickedCell != null){
						command.clickedCell(clickedCell);
					}
				}
			}
		}
	}


}
