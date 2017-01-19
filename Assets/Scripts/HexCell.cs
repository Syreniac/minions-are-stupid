using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {

	private int x, y;
	private float fx, fy;
	private float height;
	private HexGrid grid;

	private Mesh collisionMesh;
	private List<Vector3> localvertices;
	private List<int> localtriangles;
	private List<Color> localcolors;

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
		GetComponent<MeshFilter>().mesh = collisionMesh = new Mesh();
		height = 0f;
		localvertices = new List<Vector3>();
		localtriangles = new List<int>();
		localcolors = new List<Color>();
	}

	public void Rerender(List<Vector3> vertices, List<int> triangles, List<Color> colors){
		localvertices.Clear();
		localtriangles.Clear();
		localcolors.Clear();
		Vector3 center = new Vector3(fx, height, fy);
		for(int i = 0; i < 6; i++)
		{
			Vector3 topCorner1 = center + HexMetrics.corners[i];
			Vector3 topCorner2 = center + HexMetrics.corners[i + 1];
			AddTriangle(vertices, triangles, center, topCorner1, topCorner2);
			AddTriangle(localvertices, localtriangles, center, topCorner1, topCorner2);
			Vector3 bottomCorner1 = new Vector3(topCorner1.x, 0f, topCorner1.z);
			Vector3 bottomCorner2 = new Vector3(topCorner2.x, 0f, topCorner2.z);
			AddTriangle(vertices, triangles, topCorner2, topCorner1, bottomCorner1);
			AddTriangle(vertices, triangles, bottomCorner2, topCorner2, bottomCorner1);
			AddTriangle(localvertices, localtriangles, topCorner2, topCorner1, bottomCorner1);
			AddTriangle(localvertices, localtriangles, bottomCorner2, topCorner2, bottomCorner1);
		}
		collisionMesh.vertices = localvertices.ToArray();
		collisionMesh.triangles = localtriangles.ToArray();
		collisionMesh.RecalculateNormals();
		collisionMesh.colors = localcolors.ToArray();
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
}
