using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUtil{

	static private PathfindingConditional checkBlank;
	static private PathfindingCalculator calculateBlank;

	static PathfindingUtil(){
		checkBlank = new BlankPathfindingConditional();
		calculateBlank = new BlankPathfindingCalculator();
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
		return findPath(null, start, destination, checkBlank, calculateBlank);
	}

	public static List<HexCell> findPath(BaseUnit unit, HexCell start, HexCell destination, PathfindingConditional conditional, PathfindingCalculator calculation){
		
		Dictionary<HexCell, HexCellPathfinding> pathfindingValues = new Dictionary<HexCell, HexCellPathfinding>();
		FastPriorityQueue<HexCell> open = new FastPriorityQueue<HexCell>(999);
		List<HexCell> closed = new List<HexCell>();

		HexCellPathfinding startNode = new HexCellPathfinding{
			cell = start,
			parent=null,
			costSoFar=0,
			distanceRemaining=PathfindingUtil.hexGridDistance(start, destination)
		};

		HexCellPathfinding closest = startNode;

		open.Enqueue(start, PathfindingUtil.hexGridDistance(start, destination));
		pathfindingValues.Add(start, startNode);

		while(open.Count != 0){
			HexCell first = open.Dequeue();

			if(pathfindingValues[first].distanceRemaining < closest.distanceRemaining){
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
				if(!closed.Contains(cell) && conditional.check(unit, cell, pathfindingValues[first].cell)){
					int calculatedCost = calculation.calculate(unit, cell, pathfindingValues[first].cell);
					if(!open.Contains(cell)){

						int hexDistance = hexGridDistance(destination, cell);
						if(hexDistance < 100){
							open.Enqueue(cell, hexGridDistance(destination, cell));
							pathfindingValues.Add(cell, new HexCellPathfinding{
								cell = cell,
								costSoFar = pathfindingValues[first].costSoFar + calculatedCost,
								distanceRemaining = hexDistance,
								parent = pathfindingValues[first]
							});	
						}
					}
					else{
						if(pathfindingValues[cell].costSoFar > pathfindingValues[first].costSoFar+calculatedCost){
							pathfindingValues[cell].costSoFar = pathfindingValues[first].costSoFar+calculatedCost;
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
	public static HexCell findClosestOpenCell(BaseUnit unit, HexCell target, out int distance, PathfindingConditional conditional){
		List<HexCell> toTest = new List<HexCell>();

		toTest.Add(target);
		int i = 0;

		while(i < toTest.Count){
			if(conditional.check(unit, toTest[i], target)){
				distance = PathfindingUtil.hexGridDistance(target, toTest[i]);
				return toTest[i];
			}
			toTest[i].addNeighbourCells(toTest);
			i++;
		}
		distance = 0;
		return null;
	}

	private class HexCellPathfinding{
		public HexCell cell;
		public int f{get{return costSoFar+distanceRemaining;}}
		public int costSoFar;
		public int distanceRemaining;
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
