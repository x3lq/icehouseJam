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
	public List<AudioClip> randomGrowl;
	public List<AudioClip> damage;
	public AudioClip death;
	public AudioClip horn;
	private bool dead;

	private AudioSource source;

	// Start is called before the first frame update
	void Start()
    {
		source = GetComponent<AudioSource>();

		StartCoroutine(RandomNoise());
    }

	void PlayOneShot(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}

	IEnumerator RandomNoise()
	{
		if (!dead)
		{
			yield return new WaitForSeconds(Random.Range(5, 8));
			PlayOneShot(randomGrowl[Random.Range(0, randomGrowl.Count)]);

			StartCoroutine(RandomNoise());
		}
	}
 
	public void PlayGrowl()
	{
		PlayOneShot(growl);
	}

	public void Intro()
	{
		StartCoroutine(IntroCoroutine());
	}

	private IEnumerator IntroCoroutine()
	{
		PlayOneShot(horn);

		yield return new WaitForSeconds(7);

		AudioManager.current.selection = AudioManager.Tracks.boss;
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

	public void PlayDamage()
	{
		PlayOneShot(damage[Random.Range(0, damage.Count)]);
	}

	public void PlayDeath()
	{
		PlayOneShot(death);
	}
}
