using UnityEngine;
using System.Collections;

public class Buzzer : MonoBehaviour {

	[SerializeField] Flasher flasher;
	[SerializeField] bool buzzing = false;
	[SerializeField] AudioSource speaker;
	[SerializeField] KeyCode actionKey;
	int position = 0;
	int sampleRate = 44100;
	[SerializeField, Range(20, 1000)] float frequencyA = 440;
	[SerializeField, Range(20, 1000)] float frequencyB = 440;

	void Start() {
		AudioClip myClip = AudioClip.Create ("Buzz", sampleRate * 2, 1, sampleRate, true, OnAudioRead, OnAudioSetPosition);
		speaker.clip = myClip;
		speaker.loop = true;
		speaker.Play ();
	}

	void OnEnable() {
		flasher.OnToggleState += Flasher_OnToggleState;
	}

	void Flasher_OnToggleState (bool isOn)
	{
		if (isOn && buzzing)
			speaker.UnPause ();
		else
			speaker.Pause ();
	}

	void OnDisable() {
		flasher.OnToggleState -= Flasher_OnToggleState;
	}

	void OnAudioRead(float[] data) {
		int count = 0;
		while (count < data.Length) {
			data [count] = Mathf.Sign (
				Mathf.Sin (2 * Mathf.PI * frequencyA * position / sampleRate) +
				Mathf.Sin (2 * Mathf.PI * frequencyB * position / sampleRate));
			position++;
			count++;
		}
	}

	void OnAudioSetPosition(int newPosition) {
		position = newPosition;
	}

	void Update() {
		if (Input.GetKeyDown (actionKey))
			buzzing = !buzzing;
	}
}
