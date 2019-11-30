using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager current;

	public enum Tracks { ambience, ambienceWithMusic, boss, lastHall }
	public Tracks selection;
	private Tracks lastSelection;

	[HideInInspector()]
	public GameObject boss;
	[HideInInspector()]
	public GameObject character;

	[Header("Settings")]
	public float fadeSpeed;
	public float bossAmbienceTriggerDistance;

	[Header("Tracks")]
	public AudioSource ambienceSource;
	public AudioSource ambienceWithMusicSource;
	public AudioSource bossSource;
	public AudioSource lastHallSource;

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(gameObject);
		current = this;
		lastSelection = Tracks.lastHall;
	}

    // Update is called once per frame
    void Update()
    {
		CheckBossTriggerDistance();

		SwitchBasedOnSelection();

		lastSelection = selection;
    }

	void SwitchBasedOnSelection()
	{
		switch (selection)
		{
			case Tracks.ambience:
				if (ambienceSource.volume == 1)
				{
					return;
				}

				ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, 1, Time.deltaTime * fadeSpeed);
				ambienceWithMusicSource.volume = Mathf.MoveTowards(ambienceWithMusicSource.volume, 0, Time.deltaTime * fadeSpeed);
				bossSource.volume = Mathf.MoveTowards(bossSource.volume, 0, Time.deltaTime * fadeSpeed);
				lastHallSource.volume = Mathf.MoveTowards(lastHallSource.volume, 0, Time.deltaTime * fadeSpeed);
				if (selection == lastSelection)
				{
					return;
				}
				ambienceSource.Play();
				break;

			case Tracks.ambienceWithMusic:
				if (ambienceWithMusicSource.volume == 1)
				{
					return;
				}
				ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, 0, Time.deltaTime * fadeSpeed);
				ambienceWithMusicSource.volume = Mathf.MoveTowards(ambienceWithMusicSource.volume, 1, Time.deltaTime * fadeSpeed);
				bossSource.volume = Mathf.MoveTowards(bossSource.volume, 0, Time.deltaTime * fadeSpeed);
				lastHallSource.volume = Mathf.MoveTowards(lastHallSource.volume, 0, Time.deltaTime * fadeSpeed);
				if (selection == lastSelection)
				{
					return;
				}
				ambienceWithMusicSource.Play();
				break;

			case Tracks.boss:
				if (bossSource.volume == 1)
				{
					return;
				}
				ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, 0, Time.deltaTime * fadeSpeed);
				ambienceWithMusicSource.volume = Mathf.MoveTowards(ambienceWithMusicSource.volume, 0, Time.deltaTime * fadeSpeed);
				bossSource.volume = Mathf.MoveTowards(bossSource.volume, 1, Time.deltaTime * fadeSpeed);
				lastHallSource.volume = Mathf.MoveTowards(lastHallSource.volume, 0, Time.deltaTime * fadeSpeed);
				if (selection == lastSelection)
				{
					return;
				}
				bossSource.Play();
				break;

			case Tracks.lastHall:
				if (lastHallSource.volume == 1)
				{
					return;
				}
				ambienceSource.volume = Mathf.MoveTowards(ambienceSource.volume, 0, Time.deltaTime * fadeSpeed);
				ambienceWithMusicSource.volume = Mathf.MoveTowards(ambienceWithMusicSource.volume, 0, Time.deltaTime * fadeSpeed);
				bossSource.volume = Mathf.MoveTowards(bossSource.volume, 0, Time.deltaTime * fadeSpeed);
				lastHallSource.volume = Mathf.MoveTowards(lastHallSource.volume, 1, Time.deltaTime * fadeSpeed);
				if (selection == lastSelection)
				{
					return;
				}
				lastHallSource.Play();
				break;
		}
	}

	void CheckBossTriggerDistance()
	{
		if (!boss || !character)
		{
			return;
		}

		if (!(selection == Tracks.boss) && (boss.transform.position - character.transform.position).magnitude <= bossAmbienceTriggerDistance)
		{
			selection = Tracks.ambience;
		}

		if (!(selection == Tracks.boss) && (boss.transform.position - character.transform.position).magnitude > bossAmbienceTriggerDistance)
		{
			selection = Tracks.ambienceWithMusic;
		}
	}
}
