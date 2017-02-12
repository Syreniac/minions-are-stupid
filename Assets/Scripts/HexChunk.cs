using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexChunk : MonoBehaviour {

	private int startX, startY, endX, endY;

	private HexGrid grid;

	private Mesh mesh;
	private List<Vector3> vertices;
	private List<int> triangles;
	private List<Color> colors;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Make(HexGrid grid, int x, int y){
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		this.grid = grid;
		startX = x * HexMetrics.ChunkWidth;
		startY = y * HexMetrics.ChunkHeight;
		endX = startX + HexMetrics.ChunkWidth;
		endY = startY + HexMetrics.ChunkHeight;
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
		RerenderCells();
	}

	public void RerenderCells(){
		int x,y;
		vertices.Clear();
		triangles.Clear();
		colors.Clear();
		for(x = startX; x < endX; x++){
			for(y = startY; y < endY; y++){
				grid.cells[x,y].Rerender(vertices, triangles, colors);
			}
		}
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		mesh.colors = colors.ToArray();
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	public void ParentCells() {
		for(int x = startX; x < endX; x++){
			for(int y = startY; y < endY; y++){
				grid.cells[x,y].transform.SetParent(gameObject.transform);
			}
		}
	}

	public HexCell subtestCollision(Ray ray){
		HexCell cell = null;
		float distance = Mathf.Infinity;
		for(int x = startX; x < endX; x++){
			for(int y = startY; y < endY; y++){
				float hitDistance = grid.cells[x,y].subtestCollision(ray);
				if(hitDistance < distance){
					cell = grid.cells[x,y];
					distance = hitDistance;
				}
			}
		}
		return cell;
	}

	void Collide(){
		// Do collision
	}
}
