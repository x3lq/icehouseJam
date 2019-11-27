using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
	[Header("Objects")]
	public WorldGenerator generator;

	[Header("Status")]
	public bool playerLoaded;
	public WorldTile currentTile;
	public List<WorldTile> neighbourTiles;

	[Header("Info")]
	public int playerLayer;
	public Transform playerTransform;

	public static bool worldGenerated;

    // Start is called before the first frame update
    void Start()
    {
		neighbourTiles = new List<WorldTile>();

	}

    // Update is called once per frame
    void Update()
    {
		if (worldGenerated)
		{
			if (!playerLoaded)
			{
				currentTile = generator.layers[0].tiles[generator.sizeX / 2, generator.sizeY / 2];
				currentTile.active = true;
				playerLayer = currentTile.layer;
				playerTransform.position = currentTile.transform.position;
				Camera.main.transform.position = playerTransform.position + Vector3.back * 10;
				playerLoaded = true;

				LoadNeighbourTiles();
			}

			LoadCurrentTile();
		}
    }

	void LoadCurrentTile()
	{
		Vector3 tilePosition = generator.GetTilePositionFromWorldPosition(playerTransform.position);

		if (tilePosition != currentTile.transform.position)
		{
			WorldTile tile = null;
			foreach (WorldTile neighbourTile in neighbourTiles)
			{
				if (tilePosition == neighbourTile.transform.position)
				{
					tile = neighbourTile;
				}
			}

			if (tile && tile != currentTile)
			{
				if (currentTile)
				{
					currentTile.active = false;
				}
				tile.active = true;
				currentTile = tile;
				playerLayer = currentTile.layer;

				LoadNeighbourTiles();
			}
		}
	}

	void LoadNeighbourTiles()
	{
		if (currentTile)
		{
			foreach (WorldTile tile in neighbourTiles)
			{
				if (tile != currentTile)
				{
					tile.active = false;
				}
			}

			neighbourTiles = new List<WorldTile>();

			if (currentTile.up >= 0)
			{
				WorldTile tile = generator.layers[currentTile.up].GetNeighbour(currentTile, Direction.up);
				tile.active = true;
				neighbourTiles.Add(tile);
			}
			if (currentTile.down >= 0)
			{
				WorldTile tile = generator.layers[currentTile.down].GetNeighbour(currentTile, Direction.down);
				tile.active = true;
				neighbourTiles.Add(tile);
			}
			if (currentTile.left >= 0)
			{
				WorldTile tile = generator.layers[currentTile.left].GetNeighbour(currentTile, Direction.left);
				tile.active = true;
				neighbourTiles.Add(tile);
			}
			if (currentTile.right >= 0)
			{
				WorldTile tile = generator.layers[currentTile.right].GetNeighbour(currentTile, Direction.right);
				tile.active = true;
				neighbourTiles.Add(tile);
			}
		}
	}
}
