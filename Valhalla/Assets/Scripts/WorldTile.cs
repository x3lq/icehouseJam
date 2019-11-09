using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
	public Vector2 size;
	public int layer;

	[Header("Attributes")]
	public int up, down, left, right;

	private void OnDrawGizmos()
	{
		// Only Draw if selected in WorldGenerator
		if (WorldGenerator.staticDisplayLayer == layer)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0.5f));
		}
	}
}
