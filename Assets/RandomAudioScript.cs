using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class RandomAudioScript : MonoBehaviour {

	public AudioClip[] clips;
	public AudioMixerGroup output;
	private AudioSource source;
	public AudioSource mainSound;
	private int previousClipNumber = 0;

	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (mainSound.isPlaying) {
			source.volume = 0.1f;
		} else if (!mainSound.isPlaying && source.isPlaying) {
			if (source.clip.name == "chillin-music") {
				source.volume = 1.0f;
			} else if (source.clip.name == "wateringplants") {
				source.volume = 0.5f;
			} else if (source.clip.name == "DJ Khaled 1") {
				source.volume = 0.5f;
			} else {
				source.volume = 0.1f;
			}
		} else {
			if (Random.Range (0, 50) == 22) {
				PlaySound ();
			}
		}
	}

	void PlaySound() {
		// randomize
		int randomClip;
		//do {
		randomClip = Random.Range (0, clips.Length);
		//} while (randomClip == previousClipNumber);
		/*if (previousClipNumber != randomClip) {
			Debug.Log ("clip number: " + randomClip.ToString () + " --- previous clip: " + previousClipNumber.ToString());
			previousClipNumber = randomClip;*/
			// get an AudioSource
		/*AudioSource source = gameObject.GetComponent<AudioSource> ();
		if (!source.isPlaying) {*/
		if (previousClipNumber != randomClip) {
			previousClipNumber = randomClip;
			// load clip into the AudioSource
			source.clip = clips [randomClip];

			// set the output for AudioSource
			source.outputAudioMixerGroup = output;

			if (source.clip.name == "chillin-music") {
				source.volume = 1.0f;
			} else if (source.clip.name == "wateringplants") {
				source.volume = 0.5f;
			} else if (source.clip.name == "DJ Khaled 1") {
				source.volume = 0.5f;
			} else {
				source.volume = 0.1f;
			}

			// play the clip
			source.Play ();
		}

		// destroy the AudioSource
		//Destroy(source, clips[randomClip].length);
	}
}
