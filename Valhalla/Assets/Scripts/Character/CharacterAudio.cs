using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
	private AudioSource source;

	public AudioClip dash;
	public AudioClip axtHit;
	public AudioClip axtThrow;

    // Start is called before the first frame update
    void Start()
    {
		source = GetComponent<AudioSource>();
    }

	void PlayOneShot(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}

	public void PlayDashSound()
	{
		source.PlayOneShot(dash, 0.6f);
	}

	public void PlayAxtHitSound()
	{
		source.PlayOneShot(axtHit);
	}

	public void PlayAxtThrow()
	{
		source.PlayOneShot(axtThrow, 0.3f);
	}
}
