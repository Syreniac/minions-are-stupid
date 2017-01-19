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

	// Use this for initialization
	void Start () {
		widthInChunks = HexMetrics.WidthInChunks;
		heightInChunks = HexMetrics.HeightInChunks;
		widthInCells = widthInChunks * HexMetrics.ChunkWidth;
		heightInCells = heightInChunks * HexMetrics.ChunkHeight;

		chunks = new HexChunk[widthInChunks,heightInChunks];
		cells = new HexCell[widthInCells, heightInCells];
		MakeCells();
		MakeChunks();
		//MakeMeshForAllChunks();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeChunks(){
		int x,y;
		for(x = 0; x < widthInChunks; x++){
			for(y = 0; y < heightInChunks; y++){
				chunks[x,y] = Instantiate<HexChunk>(hexChunkPrefab); 
				MakeChunk(x,y);
			}
		}
	}

	void MakeChunk(int x, int y){
		chunks[x,y].Make(this,x,y);
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
				cells[x,y].Make(this,x,y);
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

}
