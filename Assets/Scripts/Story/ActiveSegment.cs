using UnityEngine;
using System.Collections;

public delegate void FoundKey();
public delegate void CompletedSegment();
public delegate void CompletedEpisode();
public delegate void CompletedGame();
public delegate void Miss(float distance);
public delegate void RestartSegment();
public delegate void EpisodeStarted();

public class ActiveSegment : MonoBehaviour {

	public event FoundKey OnFoundKey;
	public event CompletedSegment OnCompletedSegment;
	public event Miss OnMiss;
	public event RestartSegment OnRestartSegment;
	public event CompletedEpisode OnCompletedEpisode;
	public event EpisodeStarted OnEpisodeStarted;
	public event CompletedGame OnCompletedGame;

	[SerializeField] KeyBoard.Board board;
	[SerializeField] Episode[] episodes;
	[SerializeField, Range(-1, 10)] int maxMissPerKey;
	[SerializeField] ChoiceBank bank;

	int currentMisses = 0;
	Episode currentEpisode;
	StorySegment segment;

	void Update() {
		if (segment == null) {
			if (currentEpisode == null)
				currentEpisode = episodes [0];
			segment = currentEpisode.entrySegment;
			if (OnCompletedEpisode != null)
				OnCompletedEpisode ();
		}
	}

	void OnEnable() {
		board.OnKeyPress += Board_OnKeyPress;
		foreach (StorySegment seg in GetComponentsInChildren<StorySegment>()) {
			seg.OnNextSegment += Seg_OnNextSegment;
			seg.OnNextEpisode += Seg_OnNextEpisode;
			seg.OnChoice += Seg_OnChoice;
		}
	}
		
	void OnDisable() {
		board.OnKeyPress -= Board_OnKeyPress;
		foreach (StorySegment seg in GetComponentsInChildren<StorySegment>()) {
			seg.OnNextSegment -= Seg_OnNextSegment;
			seg.OnNextEpisode -= Seg_OnNextEpisode;
			seg.OnChoice -= Seg_OnChoice;
		}

	}

	void Seg_OnChoice (string name, string value)
	{
		bank.RegisterChoice (currentEpisode.index, name, value);
		Debug.Log(string.Format("Chose {0} = {1}", name, value));
	}

	void Seg_OnNextEpisode ()
	{
		bank.OpenEpisodePage (currentEpisode.index);
		if (currentEpisode.index + 1 < episodes.Length) {
			if (OnCompletedEpisode != null)
				OnCompletedEpisode ();
			currentEpisode = episodes [currentEpisode.index + 1];
			segment = currentEpisode.entrySegment;
		} else {
			if (OnCompletedGame != null)
				OnCompletedGame ();
			currentEpisode = episodes [0];
			bank.Wipe ();
			segment = currentEpisode.entrySegment;

		}
	}

	void Seg_OnNextSegment (StorySegment next)
	{
		segment = next;
		segment.Step ("");
		if (OnCompletedSegment != null) {
			OnCompletedSegment ();
		}
	}		

	void Board_OnKeyPress (KeyBoard.Key key)
	{
		if (segment.Initiated) {
			if (segment.IsHit (key)) {			
				HandleNextPosition (key.KeyName);
			} else {
				HandleMiss (key);
			}
		} else {
			if (key.keyCode == KeyCode.Space) {
				if (OnEpisodeStarted != null)
					OnEpisodeStarted ();
				segment.Step ("");
			}
		}
	}

	void HandleNextPosition(string keyName) {
		currentMisses = 0;
		if (segment.Step (keyName)) {
			if (OnFoundKey != null)
				OnFoundKey ();
		}
	}

	void HandleMiss(KeyBoard.Key otherKey) {
		currentMisses++;
		if (currentMisses > maxMissPerKey && maxMissPerKey >= 0) {
			segment.Restart ();
			currentMisses = 0;
			if (OnRestartSegment != null)
				OnRestartSegment ();
		} else if (OnMiss != null)
			OnMiss (segment.GetDistance(otherKey));
	}
}
