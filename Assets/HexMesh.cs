using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{

	List<Mesh> collisionMeshes;
	List<HexMesh> hexMeshes;
	List<Vector3> vertices;
	List<int> triangles;
	List<MeshCollider> meshColliders;
	List<Color> colors;

	void Awake()
	{
		hexMeshes = new List<HexMesh>();
		collisionMeshes = new List<Mesh>();
		meshColliders = new List<MeshCollider>();//gameObject.AddComponent<MeshCollider>();
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colors = new List<Color>();
	}
	public void Triangulate(HexCell[] cells)
	{
		foreach(MeshCollider meshCollider in meshColliders)
		{
			MeshCollider.Destroy(meshCollider);
		}
		collisionMeshes.Clear();
		meshColliders.Clear();
		for (int i = 0; i < cells.Length / 100; i++) {
			Mesh mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;
			vertices.Clear();
			triangles.Clear();
			colors.Clear();
			int startPosition = i * 100;
			int endPosition = (i + 1) * 100;

			for (int j = startPosition; j < endPosition; j++)
			{
				Triangulate(cells[j]);
			}

			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.RecalculateNormals();
			mesh.colors = colors.ToArray();
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
			collisionMeshes.Add(mesh);
			meshColliders.Add(meshCollider);
		}
	}

	void Triangulate(HexCell cell)
	{
		Vector3 center = cell.transform.localPosition;
		Vector3 centerWithHeight = new Vector3(center.x, cell.height, center.z);
		for (int i = 0; i < 6; i++)
		{
			AddTriangle(centerWithHeight, centerWithHeight + HexMetrics.corners[i], centerWithHeight + HexMetrics.corners[i+1]);
		}
		cell.makeMesh();
	}

	void AddTriangleColor(Color color)
	{
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}
	void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
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
