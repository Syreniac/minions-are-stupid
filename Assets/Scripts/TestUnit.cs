using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour {

	HexCell hexCell;
    float height = 10f;
    List<HexCell> path;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Make(HexCell initialCell){
		hexCell = initialCell;
		Vector3 vector = hexCell.getPosition();
		int cellHeight = hexCell.getHeight();
		gameObject.transform.position = new Vector3(hexCell.fx, height + cellHeight, hexCell.fy); 
	}

	public List<GameObject> debugDrawPath(TestUnit prefab){
		List<GameObject> retur = new List<GameObject>();
		for(int i = path.Count - 1; i >= 0; i--){
			path[i].debugHexCoords();
			TestUnit temp = Instantiate<TestUnit>(prefab);
			temp.Make(path[i]);
			retur.Add(temp.gameObject);

		}
		return retur;
	}

	public void findPath(HexCell destination){
		Dictionary<HexCell, HexCellPathfinding> pathfindingValues = new Dictionary<HexCell, HexCellPathfinding>();
		FastPriorityQueue<HexCell> open = new FastPriorityQueue<HexCell>(999);
		List<HexCell> closed = new List<HexCell>();

		HexCellPathfinding start = new HexCellPathfinding{
			cell = hexCell,
			parent=null,
			g=0,
			h=hexGridDistance(hexCell, destination)
		};

		open.Enqueue(hexCell, hexGridDistance(hexCell, destination));
		pathfindingValues.Add(hexCell, start);

		while(open.Count != 0){
			HexCell first = open.Dequeue();

			if(first == destination){
				path = new List<HexCell>();
				HexCellPathfinding pathCell = pathfindingValues[first];
				while(pathCell != null){
					path.Add(pathCell.cell);
					pathCell = pathCell.parent;
				}
				return;
			}
			closed.Add(first);

			List<HexCell> neighbours = new List<HexCell>();
			first.addNeighbourCells(neighbours);

			foreach(HexCell cell in neighbours){
				if(!closed.Contains(cell) && !cell.immovable){
					if(!open.Contains(cell)){

						open.Enqueue(cell, hexGridDistance(destination, cell));
						pathfindingValues.Add(cell, new HexCellPathfinding{
							cell = cell,
							g = pathfindingValues[first].g+1,
							h = hexGridDistance(destination, cell),
							parent = pathfindingValues[first]
						});	
					}
					else{
						if(pathfindingValues[cell].g > pathfindingValues[first].g+1){
							pathfindingValues[cell].g = pathfindingValues[first].g+1;
							pathfindingValues[cell].parent = pathfindingValues[first];
						}	
					}
				}
			}
		}
	}

	private int hexGridDistance(HexCell start, HexCell end){
		return GetTileDistance(start.x, start.y, end.x, end.y);
	}

	private int GetTileDistance(int aX1, int aY1, int aX2, int aY2)
	{
	    int dx = aX2 - aX1;     // signed deltas
	    int dy = aY2 - aY1;
	    int x = Mathf.Abs(dx);  // absolute deltas
	    int y = Mathf.Abs(dy);
	    // special case if we start on an odd row or if we move into negative x direction
	    if ((dx < 0)^((aY1&1)==1))
	        x = Mathf.Max(0, x - (y + 1) / 2);
	    else
	        x = Mathf.Max(0, x - (y) / 2);
	    return x + y;
	}

	public void step(){

	}

	private class HexCellPathfinding{
		public HexCell cell;
		public int f{get{return g+h;}}
		public int g;
		public int h;
		public HexCellPathfinding parent;
	}
}
