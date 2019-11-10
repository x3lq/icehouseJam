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

	public int numberOfLayers = 4;
	public WorldLayer[] layers;

	[Header("Debug")]
	[Range(-1, 3)]
	public int displayLayer;
	public static int staticDisplayLayer;

    // Start is called before the first frame update
    void Start()
	{
		CreateLayers();

		CreateMap();
	}

    // Update is called once per frame
    void Update()
    {
		staticDisplayLayer = displayLayer;
    }

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

	void CreateMap()
	{
		WorldTile baseTile = layers[0].tiles[sizeX / 2, sizeY / 2];

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
				List<int> possibleLayers = GetPossibleLayers(tile, direction);

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
	}

	List<int> GetPossibleLayers(WorldTile tile, Direction direction)
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

		Debug.Log(possibleLayers.Count);

		return possibleLayers;
	}

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

	void MakeConnection(WorldTile tile, Direction direction, int layer)
	{
		tile.Connect(direction, layer);

		Vector2 positionOfConnectTile = (Vector2)tile.transform.position + Util.GetVectorFromDirection(direction) * tileSize;
		WorldTile connectTilelayers = layers[layer].GetTileAtWorldPosition(positionOfConnectTile);

		connectTilelayers.Connect(Util.GetOppositeDirection(direction), tile.layer);
	}

}
