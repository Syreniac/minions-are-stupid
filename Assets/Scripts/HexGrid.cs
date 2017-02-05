using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public HexCell[,] cells;

	public int widthInChunks, heightInChunks;

	private int widthInCells, heightInCells;

	private HexChunk[,] chunks;

	public HexChunk hexChunkPrefab;

	public HexCell hexCellPrefab;

	// Initialised in MakeChunks()
	public HexChunk highlightedChunk;

	// Use this for initialization
	void Start () {
		widthInChunks = HexMetrics.WidthInChunks;
		heightInChunks = HexMetrics.HeightInChunks;
		widthInCells = widthInChunks * HexMetrics.ChunkWidth;
		heightInCells = heightInChunks * HexMetrics.ChunkHeight;

		chunks = new HexChunk[widthInChunks,heightInChunks];
		cells = new HexCell[widthInCells, heightInCells];
		MakeCells();
		GenerateTerrain();
		MakeChunks();
		//MakeMeshForAllChunks();
	}

	void MakeChunks(){
		int x,y;
		for(x = 0; x < widthInChunks; x++){
			for(y = 0; y < heightInChunks; y++){
				chunks[x,y] = Instantiate<HexChunk>(hexChunkPrefab);
				chunks[x,y].name = "Chunk " + x + "," + y;
				MakeChunk(x,y);
			}
		}
		highlightedChunk = chunks[0,0];
	}

	void MakeChunk(int x, int y){
		chunks[x,y].Make(this,x,y);
		chunks[x,y].ParentCells();
	}

	/*void MakeMeshForAllChunks(){
		int x,y;
		for(x = 0; x< width; x++){
			for(y = 0; y < height; y++){
				chunks[x][y].MakeMesh();
			}
		}
	}*/

	void MakeCells(){
		int x,y;
		for(x = 0; x< widthInCells; x++){
			for(y = 0; y < heightInCells; y++){
				cells[x,y] = Instantiate<HexCell>(hexCellPrefab);
				cells[x,y].name = "Cell " + x + "," + y;
				cells[x,y].Make(this,x,y);
			}
		}
	}

	void GenerateTerrain(){

		for(int i = 0; i < Random.Range(60, 100); i++){
			if(Random.Range(0, 100) > 20){
				AddHeightToCircle(Random.Range(0, widthInCells), Random.Range(0,heightInCells),
									  Random.Range(8, 16), Random.Range(20,50));
			}
			else{
				ApplyHeightToCircle(Random.Range(0, widthInCells), Random.Range(0,heightInCells),
									  Random.Range(8, 16), 1);
			}
		}

		for(int i = 0; i < 10; i++){
			SmoothTerrain();
		}
	}

	void SmoothTerrain(){
		int[,] newHeights = new int[widthInCells, heightInCells];
		for(int x = 0; x < widthInCells; x++){
			for(int y = 0; y < heightInCells; y++){
				newHeights[x,y] = cells[x,y].smooth();
			}
		}
		for(int x = 0; x < widthInCells; x++){
			for(int y = 0; y < heightInCells; y++){
				cells[x,y].setHeight(newHeights[x,y]);
			}
		}
	}


	void ApplyHeightToCircle(int centerx, int centery, int diameter, int height){
		for(int x = 0; x < diameter; x++){
			for(int y = 0; y < diameter; y++){
				int x2 = (x - diameter/2);
				int y2 = (y - diameter/2);
				if(x2*x2 + y2*y2 < diameter/2*diameter/2){
					if(centerx + x > 0 && centerx + x < widthInCells && centery + y > 0 && centery + y < heightInCells){
						cells[centerx + x, centery + y].setHeight(height);
					}
				}
			}
		}
	}

	void AddHeightToCircle(int centerx, int centery, int diameter, int height){
		for(int x = 0; x < diameter; x++){
			for(int y = 0; y < diameter; y++){
				int x2 = (x - diameter/2);
				int y2 = (y - diameter/2);
				if(x2*x2 + y2*y2 < diameter/2*diameter/2){
					if(centerx + x > 0 && centerx + x < widthInCells && centery + y > 0 && centery + y < heightInCells){
						cells[centerx + x, centery + y].setHeight(cells[centerx + x, centery + y].getHeight() + height);
					}
				}
			}
		}
	}

	void SetHeightInChunks(int height){
		heightInChunks = height;
		heightInCells = height * HexMetrics.ChunkHeight;
	}

	void SetWidthInChunks(int width){
		widthInChunks = width;
		widthInCells = width * HexMetrics.ChunkWidth;
	}

	void Update () {
		Debug.Log("update");
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("input");
			HandleInput();
		}
		HighlightHoveredCell();
	}

	void HighlightHoveredCell(){
	    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
	    if(!screenRect.Contains(Input.mousePosition)){
	    	return;
	    }
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chunks"))) {
			HexChunk chunk = hit.collider.gameObject.GetComponent<HexChunk>();
			HexCell cell = chunk.subtestCollision(inputRay);
			if(cell != null){
				cell.setHighlighted();
				if(chunk != highlightedChunk){
					highlightedChunk.RerenderCells();
					highlightedChunk = chunk;
				}
				Debug.Log(cell.name);
				chunk.RerenderCells();
			}
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chunks"))) {
			Debug.Log(hit.collider.gameObject.name);
			HexChunk chunk = hit.collider.gameObject.GetComponent<HexChunk>();
			HexCell cell = chunk.subtestCollision(inputRay);
			if(cell != null){
				cell.recursivelyAdjustHeight(20, 5, 1);

				chunk.RerenderCells();
			}
		}
	}
}
