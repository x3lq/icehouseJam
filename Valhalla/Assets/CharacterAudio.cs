using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
	private AudioSource source;

	public AudioClip footstep;

    // Start is called before the first frame update
    void Start()
    {
		source = GetComponent<AudioSource>();
    }

	void PlayOneShot(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}
}
