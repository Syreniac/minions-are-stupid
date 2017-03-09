using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

enum TurnPhase { USERINPUT, EXECUTION, REACTION, GAMEOVER };

	private TurnPhase phase;

	// Phase controllers
	private UserInputController userInputController;
	private ExecutionController executionController;
	private ReactionController 	reactionController;

// user input 

// execution

// reaction

	void Awake() {
		userInputController = GetComponent<UserInputController>();
		executionController = GetComponent<ExecutionController>();
		reactionController 	= GetComponent<ReactionController>();
	}

	void Start () {
		phase = TurnPhase.USERINPUT;
		userInputController.Active = true;
		StartCoroutine("Run");
	}

	IEnumerator Run(){
		while (phase != TurnPhase.GAMEOVER) {
			switch(phase) {
				case TurnPhase.USERINPUT:
					if(!userInputController.Active){
						executionController.Active = true;
						phase = TurnPhase.EXECUTION;
					}
					break;
				case TurnPhase.EXECUTION:
					executionController.run();
					reactionController.Active = true;
					phase = TurnPhase.REACTION;
					reactionController.begin();
					break;
				case TurnPhase.REACTION:
					if(!reactionController.Active){
						userInputController.Active = true;
						phase = TurnPhase.USERINPUT;
					}
					break;
			}
       		yield return null;
		}	
	}
}	
