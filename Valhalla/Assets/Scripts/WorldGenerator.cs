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
	[Range(-1, 4)]
	public int displayLayer;
	public static int staticDisplayLayer;

    // Start is called before the first frame update
    void Start()
	{
		CreateLayers();
	}

    // Update is called once per frame
    void Update()
    {
		staticDisplayLayer = displayLayer;
    }

	void CreateLayers()
	{
		layers = new WorldLayer[numberOfLayers];

		Vector3 startPosition = -Vector3.right * sizeX / 2f * tileSize.x + Vector3.right * tileSize.x / 2f * (sizeX%2) - Vector3.up * sizeY / 2f * tileSize.y + Vector3.up * tileSize.y /2f * (sizeY%2);

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
		}
	}
}
