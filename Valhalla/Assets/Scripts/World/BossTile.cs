using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTile : MonoBehaviour
{
	public WorldTile tile;

	public GameObject leftBlock, rightBlock, bottomBlock, topAccess;

    // Start is called before the first frame update
    void Start()
    {
		Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void Setup()
	{
		if (tile.up < 0)
		{
			topAccess.SetActive(false);
		}

		if (tile.down < 0)
		{
			bottomBlock.SetActive(true);
		}

		if (tile.left < 0)
		{
			leftBlock.SetActive(true);
		}

		if (tile.right < 0)
		{
			rightBlock.SetActive(true);
		}


		GetComponent<CompositeCollider2D>().GenerateGeometry();
	}
}
