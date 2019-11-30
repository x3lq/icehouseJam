using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GoblinAudio : MonoBehaviour
{
	public AudioClip growl;
	public AudioClip landing;
	public AudioClip smash;
	public AudioClip rockSmash;
	public AudioClip randomGrowl;

	private AudioSource source;

	// Start is called before the first frame update
	void Start()
    {
		source = GetComponent<AudioSource>();

		StartCoroutine(RandomNoise());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void PlayOneShot(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}

	IEnumerator RandomNoise()
	{
		yield return new WaitForSeconds(Random.Range(2, 5));

		PlayOneShot(randomGrowl);

		StartCoroutine(RandomNoise());
	}
 
	public void PlayGrowl()
	{
		PlayOneShot(growl);
	}

	public void PlayLanding()
	{
		PlayOneShot(landing);
	}

	public void PlaySmash()
	{
		PlayOneShot(smash);
	}

	public void PlayRockSmash()
	{
		PlayOneShot(rockSmash);
	}
}
