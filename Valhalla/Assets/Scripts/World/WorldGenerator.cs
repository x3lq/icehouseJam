using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	public GameObject worldLayerPrefab;

	[Header("Settings")]
	public int sizeX;
	public int sizeY;
	public Vector2 tileSize;
	public int bossTileDistance;

	public int numberOfLayers = 4;
	public WorldLayer[] layers;

	[Header("Debug")]
	public bool drawGridGizmos;
	public static bool staticDrawGridGizmos;
	[Range(-1, 3)]
	public int displayLayer;
	public static int staticDisplayLayer;

    // Start is called before the first frame update
    void Start()
	{
		CreateLayers();

		CreateMap();

		SetBossTile();

		SetupTiles();
	}

    // Update is called once per frame
    void Update()
    {
		staticDisplayLayer = displayLayer;
		staticDrawGridGizmos = drawGridGizmos;
    }

	// Creates all the layers needed for the map and populates them
	void CreateLayers()
	{
		layers = new WorldLayer[numberOfLayers];

		Vector3 startPosition = Vector3.zero + Vector3.right * tileSize.x / 2f + Vector3.up * tileSize.y / 2f;

		//CreateLayers
		for (int i=0; i<numberOfLayers; i++)
		{
			WorldLayer layer = Instantiate(worldLayerPrefab, transform.position, Quaternion.identity, transform).GetComponent<WorldLayer>();
			layer.name = "Layer " + i;
			layer.layer = i;
			layer.sizeX = sizeX;
			layer.sizeY = sizeY;
			layer.tileSize = tileSize;

			layer.Populate(startPosition);

			layers[i] = layer;
		}
	}

	// Creates the whole map with all the possible connections
	void CreateMap()
	{
		WorldTile baseTile = layers[0].tiles[sizeX / 2, sizeY / 2];
		baseTile.isSpawn = true;
		baseTile.distanceFromSpawn = 0;

		List<WorldTile> open = new List<WorldTile>();
		List<WorldTile> closed = new List<WorldTile>();

		closed.Add(baseTile);
		
		ConnectBaseTile(baseTile, open);

		while (open.Count > 0)
		{
			int index = Random.Range(0, open.Count);
			WorldTile tile = open[index];
			open.RemoveAt(index);

			WorldLayer currentLayer = layers[tile.layer];

			int connectionAmount = Random.Range(1, 3);

			// Get all possible Directions in which a connection can be made
			List<Direction> possibleDirections = currentLayer.GetPossibleConnectionDirections(tile);

			// Make the specified amount of connections in random directions
			for (int i=0; i<Mathf.Min(connectionAmount, possibleDirections.Count); i++)
			{
				// Get a random direction
				index = Random.Range(0, possibleDirections.Count);
				Direction direction = possibleDirections[index];
				possibleDirections.RemoveAt(index);

				// Get all possible layers in specified direction
				List<int> possibleLayers = GetPossibleLayersInDirection(tile, direction);

				if (possibleLayers.Count > 0)
				{
					WorldLayer connectionLayer = layers[possibleLayers[Random.Range(0, possibleLayers.Count)]];

					WorldTile tileInDirection = connectionLayer.GetNeighbour(tile, direction);
					if (!closed.Contains(tileInDirection))
					{
						MakeConnection(tile, direction, connectionLayer.layer);
						open.Add(tileInDirection);
					}
				}
			}

			closed.Add(tile);
		}

		WorldLoader.worldGenerated = true;
	}

	void SetupTiles()
	{
		foreach (WorldLayer layer in layers)
		{
			layer.SetupTiles();
		}
	}

	// Returns all the possible layers for a connection and a given tile
	List<int> GetPossibleLayersInDirection(WorldTile tile, Direction direction)
	{
		Vector3 nextTilePosition = tile.transform.position + (Vector3)(Util.GetVectorFromDirection(direction) * tileSize);

		List<int> possibleLayers = new List<int>();

		for(int i=0; i<numberOfLayers; i++)
		{
			if (layers[i].GetTileAtWorldPosition(nextTilePosition).GetConnectionLayerInDirection(Util.GetOppositeDirection(direction)) < 0)
			{
				possibleLayers.Add(i);

				if (i == tile.layer)
				{
					possibleLayers.Add(i);
					possibleLayers.Add(i);
					possibleLayers.Add(i);
				}
			}
		}

		return possibleLayers;
	}

	// Connects the base tile to all the surrounding tiles on all layers
	void ConnectBaseTile(WorldTile baseTile, List<WorldTile> open)
	{
		// Make a connection to every neighbour Tile on base layer
		MakeConnection(baseTile, Direction.up, 0);
		open.Add(layers[0].GetNeighbour(baseTile, Direction.up));
		MakeConnection(baseTile, Direction.down, 0);
		open.Add(layers[0].GetNeighbour(baseTile, Direction.down));
		MakeConnection(baseTile, Direction.left, 0);
		open.Add(layers[0].GetNeighbour(baseTile, Direction.left));
		MakeConnection(baseTile, Direction.right, 0);
		open.Add(layers[0].GetNeighbour(baseTile, Direction.right));

		// Connect every neighbouring tile on different layer to baseTile but not in the other direction
		WorldTile[] neighbours = layers[1].GetNeighbours(baseTile);
		for (int i=0; i<4; i++)
		{
			if (neighbours[i])
			{
				Direction direction = Util.GetOppositeDirection((Direction)i);
				neighbours[i].Connect(direction, 0);
				open.Add(neighbours[i]);
			}
		}

		neighbours = layers[2].GetNeighbours(baseTile);
		for (int i = 0; i < 4; i++)
		{
			if (neighbours[i])
			{
				Direction direction = Util.GetOppositeDirection((Direction)i);
				neighbours[i].Connect(direction, 0);
				open.Add(neighbours[i]);
			}
		}

		neighbours = layers[3].GetNeighbours(baseTile);
		for (int i = 0; i < 4; i++)
		{
			if (neighbours[i])
			{
				Direction direction = Util.GetOppositeDirection((Direction)i);
				neighbours[i].Connect(direction, 0);
				open.Add(neighbours[i]);
			}
		}
	}

	// Connects the given tile to the neighbouring tile in the specified direction and layer
	void MakeConnection(WorldTile tile, Direction direction, int layer)
	{
		tile.Connect(direction, layer);

		Vector2 positionOfConnectTile = (Vector2)tile.transform.position + Util.GetVectorFromDirection(direction) * tileSize;
		WorldTile connectTilelayers = layers[layer].GetTileAtWorldPosition(positionOfConnectTile);

		if (connectTilelayers.distanceFromSpawn > tile.distanceFromSpawn + 1)
		{
			connectTilelayers.distanceFromSpawn = tile.distanceFromSpawn + 1;
		}

		connectTilelayers.Connect(Util.GetOppositeDirection(direction), tile.layer);
	}

	public Vector3 GetTilePositionFromWorldPosition(Vector3 worldPosition)
	{
		if (worldPosition.x < 0 || worldPosition.y < 0)
		{
			return Vector3.zero;
		}

		int indexX = (int)(worldPosition.x / tileSize.x);
		int indexY = (int)(worldPosition.y / tileSize.y);

		try
		{
			return layers[0].tiles[indexX, indexY].transform.position;
		}
		catch
		{
			return Vector3.zero;
		}
	}

	public void SetBossTile()
	{
		List<WorldTile> possibleTiles = new List<WorldTile>();

		foreach (WorldLayer layer in layers)
		{
			foreach (WorldTile tile in layer.tiles)
			{
				if (tile.distanceFromSpawn < 100 && tile.distanceFromSpawn >= bossTileDistance)
				{
					possibleTiles.Add(tile);
				}
			}
		}

		if (possibleTiles.Count > 0)
		{
			WorldTile bossTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
			bossTile.isBoss = true;
		}
		else
		{
			bossTileDistance--;
			SetBossTile();
		}

	}
}
