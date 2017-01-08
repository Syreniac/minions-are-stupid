﻿using UnityEngine;

public class HexMapEditor : MonoBehaviour
{

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;

	void Awake()
	{
		hexGrid = GetComponentInChildren<HexGrid>();
		SelectColor(0);
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
			hexGrid.TouchCell(hit.point, activeColor);
		}
	}

	public void SelectColor(int index)
	{
		activeColor = colors[index];
	}
}