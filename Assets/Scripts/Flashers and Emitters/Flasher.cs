using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public delegate void ToggleState(bool isOn);

public class Flasher : MonoBehaviour {

	public event ToggleState OnToggleState;

	[SerializeField, Range(0, 1)] float registerDuration;

	Image img;
	bool _running = true;
	bool _updating = false;

	bool[] queuedSequence = null;
	bool[] sequence = new bool[0];
	int register = -1;

	void Awake() {
		img = GetComponentInChildren<Image> ();
	}

	void OnEnable() {
		StartCoroutine (Sequencer ());
	}

	void OnDisable() {
		_running = false;
	}
		
	void OnDestroy() {
		_running = false;
	}
		
	IEnumerator<WaitForSeconds> Sequencer() {
		_running = true;
		bool prevSequence = false;
		while (_running) {
			if (!_updating) {
				if (sequence.Length == 0) {
					prevSequence = false;
					if (OnToggleState != null)
						OnToggleState (prevSequence);
					img.enabled = prevSequence;
				} else {
					register++;
					if (queuedSequence != null && register >= sequence.Length) {
						sequence = queuedSequence;
						queuedSequence = null;
						register = 0;
					}
					register %= sequence.Length;
					img.enabled = sequence [register];
					if (sequence [register] != prevSequence) {
						prevSequence = sequence [register];
						if (OnToggleState != null)
							OnToggleState (prevSequence);
					}
				}			
				yield return new WaitForSeconds (registerDuration);
			}
		}
	}

	public void SetSequence(bool[] sequence, bool[] queuedSequence) {
		_updating = true;
		this.queuedSequence = queuedSequence;
		register = -1;
		this.sequence = sequence;
		_updating = false;
		if (!_running)
			StartCoroutine (Sequencer ());

	}

	public void SetSequence(bool[] sequence) {
		_updating = true;
		queuedSequence = null;
		register = -1;
		this.sequence = sequence;
		_updating = false;
		if (!_running)
			StartCoroutine (Sequencer ());
	}
}
