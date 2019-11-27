using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileKind { fourWay, threeWayUp, threeWayDown, threeWayLeft, threeWayRight, twoWayVert, twoWayHor, twoWayLeftUp, twoWayUpRight, twoWayRightDown, twoWayDownLeft, up, down, left, right, empty, spawn, boss }

public class TilePool : MonoBehaviour
{
	public static TilePool current;

	public List<GameObject> fourWayTiles;
	public List<GameObject> threeWayUpTiles;
	public List<GameObject> threeWayDownTiles;
	public List<GameObject> threeWayLeftTiles;
	public List<GameObject> threeWayRightTiles;
	public List<GameObject> twoWayVertTiles;
	public List<GameObject> twoWayHorTiles;
	public List<GameObject> twoWayLeftUpTiles;
	public List<GameObject> twoWayUpRightTiles;
	public List<GameObject> twoWayRightDownTiles;
	public List<GameObject> twoWayDownLeftTiles;
	public List<GameObject> upTiles;
	public List<GameObject> downTiles;
	public List<GameObject> leftTiles;
	public List<GameObject> rightTiles;
	public List<GameObject> emptyTiles;
	public List<GameObject> spawnTiles;
	public List<GameObject> bossTiles;

	private void Awake()
	{
		current = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public TileKind GetTileKind(bool up, bool down, bool left, bool right)
	{
		int connections = 0 + (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);

		if (connections == 4)
		{
			return TileKind.fourWay;
		}

		if (connections == 3)
		{
			if (!up)
			{
				return TileKind.threeWayDown;
			}
			if (!down)
			{
				return TileKind.threeWayUp;
			}
			if (!left)
			{
				return TileKind.threeWayRight;
			}
			if (!right)
			{
				return TileKind.threeWayLeft;
			}
		}

		if (connections == 2)
		{
			if (!up && !down)
			{
				return TileKind.twoWayHor;
			}
			if (!left && !right)
			{
				return TileKind.twoWayVert;
			}

			if (left && up)
			{
				return TileKind.twoWayLeftUp;
			}

			if (up && right)
			{
				return TileKind.twoWayUpRight;
			}

			if (right && down)
			{
				return TileKind.twoWayRightDown;
			}

			if (down && left)
			{
				return TileKind.twoWayDownLeft;
			}
		}

		if (connections == 1)
		{
			if (up)
			{
				return TileKind.up;
			}
			if (down)
			{
				return TileKind.down;
			}
			if (left)
			{
				return TileKind.left;
			}
			if (right)
			{
				return TileKind.right;
			}
		}

		return TileKind.empty;
	}

	public int GetRandomTileVariant(TileKind kind)
	{
		switch (kind)
		{
			case TileKind.fourWay:
				return Random.Range(0, fourWayTiles.Count);
			case TileKind.threeWayDown:
				return Random.Range(0, threeWayDownTiles.Count);
			case TileKind.threeWayUp:
				return Random.Range(0, threeWayUpTiles.Count);
			case TileKind.threeWayLeft:
				return Random.Range(0, threeWayLeftTiles.Count);
			case TileKind.threeWayRight:
				return Random.Range(0, threeWayRightTiles.Count);
			case TileKind.twoWayHor:
				return Random.Range(0, twoWayHorTiles.Count);
			case TileKind.twoWayVert:
				return Random.Range(0, twoWayVertTiles.Count);
			case TileKind.twoWayLeftUp:
				return Random.Range(0, twoWayLeftUpTiles.Count);
			case TileKind.twoWayUpRight:
				return Random.Range(0, twoWayUpRightTiles.Count);
			case TileKind.twoWayRightDown:
				return Random.Range(0, twoWayRightDownTiles.Count);
			case TileKind.twoWayDownLeft:
				return Random.Range(0, twoWayDownLeftTiles.Count);
			case TileKind.up:
				return Random.Range(0, upTiles.Count);
			case TileKind.down:
				return Random.Range(0, downTiles.Count);
			case TileKind.left:
				return Random.Range(0, leftTiles.Count);
			case TileKind.right:
				return Random.Range(0, rightTiles.Count);
			case TileKind.spawn:
				return Random.Range(0, spawnTiles.Count);
			case TileKind.boss:
				return Random.Range(0, bossTiles.Count);
			default:
				return Random.Range(0, emptyTiles.Count);
		}
	}

	public GameObject GetTile(TileKind kind, int variant)
	{
		switch (kind)
		{
			case TileKind.fourWay:
				return fourWayTiles[variant];
			case TileKind.threeWayDown:
				return threeWayDownTiles[variant];
			case TileKind.threeWayUp:
				return threeWayUpTiles[variant];
			case TileKind.threeWayLeft:
				return threeWayLeftTiles[variant];
			case TileKind.threeWayRight:
				return threeWayRightTiles[variant];
			case TileKind.twoWayHor:
				return twoWayHorTiles[variant];
			case TileKind.twoWayVert:
				return twoWayVertTiles[variant];
			case TileKind.twoWayLeftUp:
				return twoWayLeftUpTiles[variant];
			case TileKind.twoWayUpRight:
				return twoWayUpRightTiles[variant];
			case TileKind.twoWayRightDown:
				return twoWayRightDownTiles[variant];
			case TileKind.twoWayDownLeft:
				return twoWayDownLeftTiles[variant];
			case TileKind.up:
				return upTiles[variant];
			case TileKind.down:
				return downTiles[variant];
			case TileKind.left:
				return leftTiles[variant];
			case TileKind.right:
				return rightTiles[variant];
			case TileKind.spawn:
				return spawnTiles[variant];
			case TileKind.boss:
				return bossTiles[variant];
			default:
				return emptyTiles[variant];
		}
	}
}
