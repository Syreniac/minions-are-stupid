using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUtil{

	static public PathfindingConditional checkBlank {get; private set;}

	static public PathfindingConditional checkUnits {get; private set;}

	static public PathfindingConditional checkTerrain {get; private set;}

	static public PathfindingConditional checkTerrainAndUnits {get; private set;}

	static PathfindingUtil(){
		checkBlank = new BlankPathfindingConditional();
		checkUnits = new UnitPathfindingConditional();
		checkTerrain = new TerrainPathfindingConditional();
		checkTerrainAndUnits = new TerrainPathfindingConditional(){next = checkUnits};
	}

	public static PathfindingConditional combinePathfindingConditionals(params PathfindingConditional[] conditionals){
		for(int i = 0; i < conditionals.Length - 1; i++){
			conditionals[i].next = conditionals[i+1];
		}
		return conditionals[0];
	}

	public static void cleanPathfindingConditionals(params PathfindingConditional[] conditionals){
		foreach(PathfindingConditional conditional in conditionals){
			conditional.next = null;
		}
	}

	public static List<HexCell> findPath_Blank(HexCell start, HexCell destination){
		return findPath(start, destination, checkBlank);
	}

	public static List<HexCell> findPath_Units(HexCell start, HexCell destination){
		return findPath(start, destination, checkUnits);
	}

	public static List<HexCell> findPath_Terrain(HexCell start, HexCell destination){
		return findPath(start, destination, checkTerrain);
	}

	public static List<HexCell> findPath(HexCell start, HexCell destination, PathfindingConditional conditional){
		
		Dictionary<HexCell, HexCellPathfinding> pathfindingValues = new Dictionary<HexCell, HexCellPathfinding>();
		FastPriorityQueue<HexCell> open = new FastPriorityQueue<HexCell>(999);
		List<HexCell> closed = new List<HexCell>();

		HexCellPathfinding startNode = new HexCellPathfinding{
			cell = start,
			parent=null,
			g=0,
			h=PathfindingUtil.hexGridDistance(start, destination)
		};

		HexCellPathfinding closest = startNode;

		open.Enqueue(start, PathfindingUtil.hexGridDistance(start, destination));
		pathfindingValues.Add(start, startNode);

		while(open.Count != 0){
			HexCell first = open.Dequeue();

			if(pathfindingValues[first].h < closest.h){
				closest = pathfindingValues[first];
			}

			if(first == destination){
				return compilePath(pathfindingValues[first]);
			}
			closed.Add(first);

			List<HexCell> neighbours = new List<HexCell>();
			first.addNeighbourCells(neighbours);

			foreach(HexCell cell in neighbours){
				Debug.Log(cell);
				if(!closed.Contains(cell) && conditional.check(cell)){
					if(!open.Contains(cell)){

						int hexDistance = hexGridDistance(destination, cell);
						if(hexDistance < 100){
							open.Enqueue(cell, hexGridDistance(destination, cell));
							pathfindingValues.Add(cell, new HexCellPathfinding{
								cell = cell,
								g = pathfindingValues[first].g+1,
								h = hexDistance,
								parent = pathfindingValues[first]
							});	
						}
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

		// We haven't found the destination :(
		return compilePath(closest);
	}

	private static List<HexCell> compilePath(HexCellPathfinding end){
		List<HexCell> path = new List<HexCell>();
		HexCellPathfinding pathCell = end;
		while(pathCell != null){
			path.Add(pathCell.cell);
			pathCell = pathCell.parent;
		}
		return path;
	}

	// Nullable
	public static HexCell findClosestOpenCell(HexCell target, out int distance, PathfindingConditional conditional){
		List<HexCell> toTest = new List<HexCell>();

		toTest.Add(target);
		int i = 0;

		while(i < toTest.Count){
			if(conditional.check(toTest[i])){
				distance = PathfindingUtil.hexGridDistance(target, toTest[i]);
				return toTest[i];
			}
			toTest[i].addNeighbourCells(toTest);
			i++;
		}
		distance = 0;
		return null;
	}

	public static HexCell findClosestOpenCell_TerrainAndUnits(HexCell target, out int distance){
		return PathfindingUtil.findClosestOpenCell(target,out distance, checkTerrainAndUnits);
	}

	private class HexCellPathfinding{
		public HexCell cell;
		public int f{get{return g+h;}}
		public int g;
		public int h;
		public HexCellPathfinding parent;
	}

	public static int hexGridDistance(HexCell start, HexCell end){
	    int dx = end.x - start.x;     // signed deltas
	    int dy = end.y - start.y;
	    int x = Mathf.Abs(dx);  // absolute deltas
	    int y = Mathf.Abs(dy);
	    // special case if we start on an odd row or if we move into negative x direction
	    if ((dx < 0)^((start.y&1)==1))
	        x = Mathf.Max(0, x - (y + 1) / 2);
	    else
	        x = Mathf.Max(0, x - (y) / 2);
	    return x + y;
	}

}
