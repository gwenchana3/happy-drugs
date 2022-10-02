using UnityEngine;

class MusicController : MonoBehaviour
{
	[SerializeField]
	AudioClip[] music = new AudioClip[0];

	[SerializeField]
	AudioSource source1 = null;
	[SerializeField]
	AudioSource source2 = null;

	bool swapped = false;
	[SerializeField]
	float maxVolume = 1;

	AudioSource currentSource
	{
		get
		{
			return swapped ? source2 : source1;
		}
	}


	AudioSource otherSource
	{
		get
		{
			return swapped ? source1 : source2;
		}
	}

	int _currentClip = 0;

	int currentClip
	{
		get
		{
			return _currentClip;
		}
		set
		{
			_currentClip = value % music.Length;
		}
	}

	[SerializeField]
	float overlapLength = 5f;
	float currentLength = 0f;

	void Update()
	{
		currentLength += Time.deltaTime;

		if (currentLength + overlapLength >= music[currentClip].length)
		{
			NextClip();
		}

		float volume = Mathf.Min(currentLength / overlapLength, 1);
		currentSource.volume = volume * maxVolume;
		otherSource.volume = (1 - volume) * maxVolume;
	}

	void NextClip()
	{
		currentClip++;
		swapped = !swapped;
		currentSource.clip = music[currentClip];
		currentSource.Play();
		currentLength = 0;
	}
}
