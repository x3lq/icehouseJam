using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { up, down, left, right }

public class Util
{
	public static Vector2 GetVectorFromDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.up:
				return Vector2.up;
			case Direction.down:
				return Vector2.down;
			case Direction.left:
				return Vector2.left;
			case Direction.right:
				return Vector2.right;
		}

		return Vector2.zero;
	}

	public static Direction GetOppositeDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.up:
				return Direction.down;
			case Direction.down:
				return Direction.up;
			case Direction.left:
				return Direction.right;
			case Direction.right:
				return Direction.left;
		}

		return Direction.up;
	}
}
