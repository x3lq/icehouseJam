﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
	public Vector2 size;
	public int layer;

	[Header("Attributes")]
	public int up, down, left, right;

	private void Awake()
	{
		up = down = left = right = -1;
	}

	private void OnDrawGizmos()
	{
		// Only Draw if selected in WorldGenerator
		if (WorldGenerator.staticDrawGridGizmos && (WorldGenerator.staticDisplayLayer == layer || WorldGenerator.staticDisplayLayer == -1))
		{
			Vector3 position = transform.position + Vector3.forward * layer;

			Gizmos.color = Color.white;
			
			if (up != -1)
			{
				int layerDifference = up - layer;
				Gizmos.DrawRay(position, Vector3.up * size.y / 2f + Vector3.forward * layerDifference/2f);
			}
			if (down != -1)
			{
				int layerDifference = down - layer;
				Gizmos.DrawRay(position, Vector3.down * size.y / 2f + Vector3.forward * layerDifference/2f);
			}
			if (left != -1)
			{
				int layerDifference = left - layer;
				Gizmos.DrawRay(position, Vector3.left * size.x / 2f + Vector3.forward * layerDifference/2f);
			}
			if (right != -1)
			{
				int layerDifference = right - layer;
				Gizmos.DrawRay(position, Vector3.right * size.x / 2f + Vector3.forward * layerDifference/2f);
			}
		}
	}

	private Color GetColorBasedOnLayer(int layer)
	{
		switch (layer)
		{
			case 0:
				return Color.blue;
			case 1:
				return Color.green;
			case 2:
				return Color.red;
			case 3:
				return Color.magenta;
			default:
				return Color.black;
		}
	}

	// Sets the connection direction to the given layer
	public void Connect(Direction direction, int layer)
	{
		switch (direction)
		{
			case Direction.up:
				up = layer;
				break;
			case Direction.down:
				down = layer;
				break;
			case Direction.left:
				left = layer;
				break;
			case Direction.right:
				right = layer;
				break;
		}
	}

	// Returns the layer of the connection in the given direction
	public int GetConnectionLayerInDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.up:
				return up;
			case Direction.down:
				return down;
			case Direction.left:
				return left;
			case Direction.right:
				return right;
			default:
				return -1;
		}
	}
}
