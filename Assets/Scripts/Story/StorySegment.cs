using UnityEngine;
using System.Collections;

public delegate void NextSegment(StorySegment next);
public delegate void NextEpisode();
public delegate void Choice(string name, string value);

public abstract class StorySegment : MonoBehaviour {

	public event Choice OnChoice;
	public event NextEpisode OnNextEpisode;
	public event NextSegment OnNextSegment;

	[SerializeField] KeyBoard.Board board;
	KeyBoard.Key[] targetKeys;

	protected void SetKeys (params string[] keyNames) {
		targetKeys = new KeyBoard.Key[keyNames.Length];
		for (int i = 0; i < keyNames.Length; i++) {
			targetKeys [i] = board.GetKey (keyNames [i]);
		}
	}

	public bool Initiated {
		get {
			return targetKeys != null;
		}
	}

	public bool IsHit(KeyBoard.Key key) {
		for (int i = 0; i < targetKeys.Length; i++) {
			if (targetKeys [i] == key)
				return true;
		}
		return false;
	}

	public float GetDistance(KeyBoard.Key key) {
		float d = -1;
		for (int i = 0; i < targetKeys.Length; i++) {
			//Debug.Log (key + " == " + targetKeys [i]);

			var new_dist = key.DistanceTo (targetKeys [i]);
			if (d < 0 || new_dist < d) {
				d = new_dist;
			} 

		}
		return d;
	}

	protected void Progress(StorySegment nextSegment) {
		targetKeys = null;
		if (nextSegment == null) {
			Debug.Log ("Requesting next episode");
			if (OnNextEpisode != null)
				OnNextEpisode ();
		} else {
			Debug.Log ("Requesting next segment: " + nextSegment);
			if (OnNextSegment != null)
				OnNextSegment (nextSegment);
		}
	}

	protected void ReportChoice(string name, string value) {
		if (OnChoice != null)
			OnChoice (name, value);
	}

	public abstract bool Step (string keyName);

	public abstract void Restart();
}
