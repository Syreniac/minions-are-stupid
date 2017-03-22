using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public HexCell[,] cells;

	public int widthInChunks, heightInChunks;

	private int widthInCells, heightInCells;

	public HexChunk[,] chunks;

	public HexChunk hexChunkPrefab;

	public HexCell hexCellPrefab;

	// Initialised in MakeChunks()
	public HexChunk highlightedChunk;

	public TestUnit testUnitPrefab;

	public TestUnit testUnitPrefab2;

	public List<BaseUnit> Units = new List<BaseUnit>();

	private float nextActionTime;

	private List<GameObject> tempDrawnPath;

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
		MakeTestUnits();
		//MakeMeshForAllChunks();
	}

	void MakeTestUnits(){
		for(int i = 0; i < 4; i++){
			BaseUnit unit = Instantiate<BaseUnit>(testUnitPrefab);
			unit.Make(cells[i,i]);
			Units.Add(unit);
		}
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
			}
		}
		for(x = 0; x < widthInCells; x++){
			for(y = 0; y < heightInCells; y++){
				cells[x,y].Make(this,x,y);
			}
		}
	}

	void GenerateTerrain(){

		for(int i = 0; i < Random.Range(HexMetrics.TerrainCirclesMin,HexMetrics.TerrainCirclesMax); i++){
			if(Random.Range(0, 100) > 20){
				AddHeightToCircle(Random.Range(0, widthInCells), Random.Range(0,heightInCells),
									  Random.Range(10, 20), Random.Range(50,100));
			}
			else{
				ApplyHeightToCircle(Random.Range(0, widthInCells), Random.Range(0,heightInCells),
									  Random.Range(2, 4), 1);
			}
		}

		for(int i = 0; i < HexMetrics.SmoothCounts; i++){
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

	public HexCell projectRay(){
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chunks"))) {
			HexChunk chunk = hit.collider.gameObject.GetComponent<HexChunk>();
			return chunk.subtestCollision(inputRay);
		}
		return null;
	}

	public HexCell projectRay(out HexChunk chunk){
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chunks"))) {
			chunk = hit.collider.gameObject.GetComponent<HexChunk>();
			return chunk.subtestCollision(inputRay);
		}
		chunk = null;
		return null;
	}

	public bool canContinue(){
		foreach(BaseUnit unit in Units){
			if(unit.canContinue()){
				return true;
			}
		}
		return false;
	}
}
