using UnityEngine;

public interface ICommand{
	void Execute();

	int getPriority();

	bool isReady();

	void clickedCell(HexCell cell, KeyCode key);
}