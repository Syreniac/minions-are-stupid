public interface ICommand{
	void Execute();

	int getPriority();

	bool isReady();

	void clickedCell(HexCell cell);
}