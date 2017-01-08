using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public Color color;
	public float height;
	Mesh collisionMesh;
	private List<Vector3> localvertices;
	private List<int> localtriangles;
	private List<Color> localcolors;

	public void Awake()
	{
		GetComponent<MeshFilter>().mesh = collisionMesh = new Mesh();
		localvertices = new List<Vector3>();
		localtriangles = new List<int>();
		localcolors = new List<Color>();
	}
	public void makeMesh()
	{
		collisionMesh.Clear();
		localvertices.Clear();
		localtriangles.Clear();
		localcolors.Clear();
		Vector3 center = new Vector3(0f, height, 0f);
		for(int i = 0; i < 6; i++)
		{
			Vector3 topCorner1 = center + HexMetrics.corners[i];
			Vector3 topCorner2 = center + HexMetrics.corners[i + 1];
			AddTriangle(center, topCorner1, topCorner2);
			Vector3 bottomCorner1 = new Vector3(topCorner1.x, 0f, topCorner1.z);
			Vector3 bottomCorner2 = new Vector3(topCorner2.x, 0f, topCorner2.z);
			AddTriangle(topCorner2, topCorner1, bottomCorner1);
			AddTriangle(bottomCorner2, topCorner2, bottomCorner1);
		}
		collisionMesh.vertices = localvertices.ToArray();
		collisionMesh.triangles = localtriangles.ToArray();
		collisionMesh.RecalculateNormals();
		collisionMesh.colors = localcolors.ToArray();
	}

	void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		int vertexIndex = localvertices.Count;
		localvertices.Add(v1);
		localvertices.Add(v2);
		localvertices.Add(v3);
		localtriangles.Add(vertexIndex);
		localtriangles.Add(vertexIndex + 1);
		localtriangles.Add(vertexIndex + 2);
	}
}
