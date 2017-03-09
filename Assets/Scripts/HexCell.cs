﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {

	private static HexCell highlightedCell = null; 

	// Array indices
	public int x, y, z;
	public float fx, fy;
	private int height;
	private HexGrid grid;
	private List<HexCell> neighbours;

	private Mesh collisionMesh;
	private List<Vector3> localvertices;
	private List<int> localtriangles;
	private List<Color> localcolors;

	public bool immovable;

	private Color color;
	private int colorHeight =99999;

	public TestUnit Unit {get; set;}

    public float Priority { get; protected internal set; }
    public int QueueIndex { get; internal set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Make(HexGrid grid, int x, int y){
		this.grid = grid;
		fx = (float) x;
		fy = (float) y;


		fx = (fx + fy * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
		fy = fy * (HexMetrics.outerRadius * 1.5f);
		this.x = x;
		this.y = y;
		this.z = -x - y;

		//immovable = Random.Range(0, 3) == 0;

		GetComponent<MeshFilter>().mesh = collisionMesh = new Mesh();
		height = 30;
		localvertices = new List<Vector3>();
		localtriangles = new List<int>();
		localcolors = new List<Color>();
		generateNeighbours();
	}

	private void generateNeighbours(){
		neighbours = new List<HexCell>();
		bool yCanGoDown = this.y > 0;
		bool xCanGoDown = this.x > 0;
		bool xCanGoUp = this.x < HexMetrics.WidthInCells - 1;
		bool yCanGoUp = this.y < HexMetrics.HeightInCells - 1;
		if(yCanGoDown){
			neighbours.Add(grid.cells[this.x,this.y - 1]);
		}
		if(yCanGoUp){
			neighbours.Add(grid.cells[this.x,this.y + 1]);
		}
		if(xCanGoDown){
			neighbours.Add(grid.cells[this.x - 1,this.y]);
		}
		if(xCanGoUp){
			neighbours.Add(grid.cells[this.x + 1,this.y]);
		}
		if(xCanGoDown && yCanGoUp && (y%2 == 0)){
			neighbours.Add(grid.cells[this.x - 1,this.y + 1]);
		}
		if(xCanGoDown && yCanGoDown && (y%2 == 0)){
			neighbours.Add(grid.cells[this.x - 1,this.y - 1]);
		}
		if(xCanGoUp && yCanGoUp && (y%2 != 0)){
			neighbours.Add(grid.cells[this.x + 1,this.y + 1]);
		}
		if(xCanGoUp && yCanGoDown && (y%2 != 0)){
			neighbours.Add(grid.cells[this.x + 1,this.y - 1]);
		}
	}

	public void Rerender(List<Vector3> vertices, List<int> triangles, List<Color> colors){
		localvertices.Clear();
		localtriangles.Clear();
		localcolors.Clear();
		float fheight = (float) Mathf.Max(20,height);
		//if(immovable){
		//	fheight = 0;	
		//}
		Vector3 center = new Vector3(fx, fheight, fy);
		for(int i = 0; i < 6; i++)
		{
			Vector3 topCorner1 = center + HexMetrics.corners[i];
			Vector3 topCorner2 = center + HexMetrics.corners[i + 1];
			AddTriangle(vertices, triangles, center, topCorner1, topCorner2);
			AddTriangleColor(colors);
			AddTriangle(localvertices, localtriangles, center, topCorner1, topCorner2);
			AddTriangleColor(localcolors);
			Vector3 bottomCorner1 = new Vector3(topCorner1.x, 0f, topCorner1.z);
			Vector3 bottomCorner2 = new Vector3(topCorner2.x, 0f, topCorner2.z);
			AddTriangle(vertices, triangles, topCorner2, topCorner1, bottomCorner1);
			AddTriangleColor(colors);
			AddTriangle(vertices, triangles, bottomCorner2, topCorner2, bottomCorner1);
			AddTriangleColor(colors);
			AddTriangle(localvertices, localtriangles, topCorner2, topCorner1, bottomCorner1);
			AddTriangleColor(localcolors);
			AddTriangle(localvertices, localtriangles, bottomCorner2, topCorner2, bottomCorner1);
			AddTriangleColor(localcolors);
		}
		collisionMesh.vertices = localvertices.ToArray();
		collisionMesh.triangles = localtriangles.ToArray();
		collisionMesh.RecalculateNormals();
		collisionMesh.colors = localcolors.ToArray();
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = collisionMesh;
	}

	void AddTriangle(List<Vector3> vertices, List<int> triangles, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	void AddTriangleColor(List<Color> colors){
		if(highlightedCell != null){
			List<HexCell> highlightedCells = new List<HexCell>();
			highlightedCells.Add(highlightedCell);
			//highlightedCell.addNeighbourCells(highlightedCells);
			if(highlightedCells.Contains(this)){
				Color tempcolor = new Color(143f/255f, 27f/255f, 196f/255f, 1f);
				colors.Add(tempcolor);
				colors.Add(tempcolor);
				colors.Add(tempcolor);
				return;
			}
		}
		if(height != colorHeight){
			colorHeight = height;
			/*if(immovable){
				color = Color.black;
			}
			else */if(height > 80f){
				color = new Color(250f/255f, 250f/255f, 250f/255f, 1f);
			}
			else if(height > 60f){
				color = new Color(212f/255f, 192f/255f, 144f/255f, 1f);
			}
			else if(height > 20f){
				// Green
				color = new Color((52f+Random.Range(-25, 25))/255f, 
								  (237f+Random.Range(-25, 25))/255f,
								  (80f+Random.Range(-25, 25))/255f,
								  1f);
			}
			else{
				color = new Color(37f/255f, 174f/255f, 230f/255f, 1f);
			}
		}
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	public float subtestCollision(Ray ray){
		RaycastHit hit;
		if(GetComponent<MeshCollider>().Raycast(ray, out hit, Mathf.Infinity)){
			return hit.distance;
		}
		return Mathf.Infinity;
	}

	public void addHeight(){
		height += 10;
	}

	public void setHighlighted(){
		highlightedCell = this;
	}

	public void setHeight(int newHeight){
		height = newHeight;	
	}

	public int getHeight(){
		return height;
	}

	public Vector3 getPosition(){
		return gameObject.transform.position;
	}

	public int smooth(){
		int[] heights = {-1,-1,-1,-1,-1,-1};
		int heightsFound = 0;
		bool yCanGoDown = this.y > 0;
		bool xCanGoDown = this.x > 0;
		bool xCanGoUp = this.x < HexMetrics.WidthInCells - 1;
		bool yCanGoUp = this.y < HexMetrics.HeightInCells - 1;
		if(yCanGoDown){
			heights[0] = grid.cells[this.x,this.y - 1].getHeight();
				heightsFound++;
		}
		if(yCanGoUp){
			heights[1] = grid.cells[this.x,this.y + 1].getHeight();
				heightsFound++;
		}
		if(xCanGoDown){
			heights[2] = grid.cells[this.x - 1,this.y].getHeight();
				heightsFound++;
		}
		if(xCanGoUp){
			heights[3] = grid.cells[this.x + 1,this.y].getHeight();
				heightsFound++;
		}
		if(xCanGoUp && yCanGoUp){
			heights[4] = grid.cells[this.x + 1,this.y + 1].getHeight();
				heightsFound++;
		}
		if(xCanGoUp && yCanGoDown){
			heights[5] = grid.cells[this.x + 1,this.y - 1].getHeight();
				heightsFound++;
		}

		int average = 0;
		foreach(int f in heights){
			if(f > 0){
				average += f;
			}
		}
		return average/heightsFound;

	}

	public void recursivelyAdjustHeight(int heightChange, int distance, int lossPerStep, float percentageEachStep = 1.0f){
		List<HexCell> cells = new List<HexCell>();
		int currentIndex = 1;
		cells.Add(this);
		height += heightChange;
		addNeighbourCells(cells);
		while(heightChange != 0 && distance > 0){
			// Black Magic
			distance--;
			heightChange = heightChange - lossPerStep;
			heightChange = (int)((float)heightChange * percentageEachStep);
			int indexToReach = cells.Count;
			for(int i = currentIndex; i < indexToReach; i++){
				cells[i].setHeight(cells[i].getHeight() + heightChange);
				cells[i].addNeighbourCells(cells);
			}
			currentIndex = indexToReach;
		}

	}

	public void debugHexCoords(){
		Debug.Log(x+","+y+","+z);
	}

	public void addNeighbourCells(List<HexCell> cells){
		foreach(HexCell cell in neighbours){
			if(!cells.Contains(cell)){
				cells.Add(cell);
			}
		}
	}

	public bool hasNeighbour(HexCell cell){
		return neighbours.Contains(cell);
	}
}
