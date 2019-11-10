﻿using System.Collections;
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
				Vector3 position = startPosition + Vector3.right * x * tileSize.x + Vector3.up * y * tileSize.y;

				WorldTile tile = Instantiate(worldTilePrefab, position, Quaternion.identity, transform).GetComponent<WorldTile>();
				tile.name = "Tile " + x + "/" + y;
				tile.size = tileSize;
				tile.layer = layer;

				tiles[x, y] = tile;
			}
		}
	}

	public WorldTile GetTileAtWorldPosition(Vector3 worldPosition)
	{
		if (worldPosition.x < 0 || worldPosition.y < 0)
		{
			return null;
		}

		int indexX = (int)(worldPosition.x / tileSize.x);
		int indexY = (int)(worldPosition.y / tileSize.y);

		Debug.Log(indexX);

		try
		{
			return tiles[indexX, indexY];
		} catch
		{
			return null;
		}
	}

	public WorldTile[] GetNeighbours(WorldTile tile)
	{
		WorldTile[] neighbours = new WorldTile[4];

		WorldTile neighbour = GetTileAtWorldPosition(tile.transform.position + Vector3.up * tileSize.y);
		if (neighbour != tile)
		{
			neighbours[0] = neighbour;
		}
		neighbour = GetTileAtWorldPosition(tile.transform.position + Vector3.down * tileSize.y);
		if (neighbour != tile)
		{
			neighbours[1] = neighbour;
		}
		neighbour = GetTileAtWorldPosition(tile.transform.position + Vector3.left * tileSize.x);
		if (neighbour != tile)
		{
			neighbours[2] = neighbour;
		}
		neighbour = GetTileAtWorldPosition(tile.transform.position + Vector3.right * tileSize.x);
		if (neighbour != tile)
		{
			neighbours[3] = neighbour;
		}

		return neighbours;
	}

	public WorldTile GetNeighbour(WorldTile tile, Direction direction)
	{
		return GetTileAtWorldPosition(tile.transform.position + (Vector3)(Util.GetVectorFromDirection(direction) * tileSize));
	}

	public List<Direction> GetPossibleConnectionDirections(WorldTile tile)
	{
		List<Direction> possibleDirections = new List<Direction>();

		if(tile.up < 0 && GetNeighbour(tile, Direction.up))
		{
			possibleDirections.Add(Direction.up);
		}
		if (tile.down < 0 && GetNeighbour(tile, Direction.down))
		{
			possibleDirections.Add(Direction.down);
		}
		if (tile.left < 0 && GetNeighbour(tile, Direction.left))
		{
			possibleDirections.Add(Direction.left);
		}
		if (tile.right < 0 && GetNeighbour(tile, Direction.right))
		{
			possibleDirections.Add(Direction.right);
		}

		return possibleDirections;
	}
}
