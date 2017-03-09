using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionController : MonoBehaviour {

	public bool Active {get; set;}

	List<ICommand> commands = new List<ICommand>();

	// Use this for initialization
	void Start () {
		Active = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void postCommand(ICommand command){
		commands.Add(command);
	}

	public void run(){
		Debug.Log("Execution controller run");
		for(int i = 0; i < commands.Count; i++){
			commands[i].Execute();
		}
		commands.Clear();
	}
}
