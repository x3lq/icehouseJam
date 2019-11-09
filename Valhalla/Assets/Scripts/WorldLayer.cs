using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayer: MonoBehaviour
{
	public GameObject worldTilePrefab;

	public int layer;

	public int sizeX, sizeY;
	public Vector2 tileSize;

	public WorldTile[,] tiles;
	
	public void Populate(Vector3 startPosition)
	{
		tiles = new WorldTile[sizeX, sizeY];

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				Vector3 position = startPosition + Vector3.right * x + Vector3.up * y;

				WorldTile tile = Instantiate(worldTilePrefab, position, Quaternion.identity, transform).GetComponent<WorldTile>();
				tile.name = "Tile " + x + "/" + y;
				tile.size = tileSize;
				tile.layer = layer;

				tiles[x, y] = tile;
			}
		}
	}
}
