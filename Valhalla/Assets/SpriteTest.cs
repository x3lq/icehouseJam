using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTest : MonoBehaviour
{

	public GameObject sprite;
	public int amount;

	public List<GameObject> sprites;

    // Start is called before the first frame update
    void Start()
    {
		sprites = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
		int count = sprites.Count;
        while(count > amount)
		{
			GameObject lastSprite = sprites[sprites.Count-1];
			Destroy(lastSprite);
			sprites.RemoveAt(sprites.Count - 1);
			count = sprites.Count;
		}

		if (count < amount)
		{
			sprites.Add(Instantiate(sprite, transform.position + Vector3.right * Random.Range(-2, 2f) + Vector3.up * Random.Range(-2, 2f), Quaternion.identity));
			count = sprites.Count;
		}
    }
}
