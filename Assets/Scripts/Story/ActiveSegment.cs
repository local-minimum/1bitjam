using UnityEngine;
using System.Collections;

public delegate void FoundKey();
public delegate void CompletedSegment();
public delegate void Miss(float distance);
public delegate void RestartSegment();

public class ActiveSegment : MonoBehaviour {

	public event FoundKey OnFoundKey;
	public event CompletedSegment OnCompletedSegment;
	public event Miss OnMiss;
	public event RestartSegment OnRestartSegment;

	[SerializeField] KeyBoard.Board board;
	[SerializeField] StorySegment entrySegment;
	[SerializeField, Range(-1, 10)] int maxMissPerKey;

	int currentMisses = 0;

	StorySegment segment;

	void Update() {
		if (segment == null) {
			segment = entrySegment;
			segment.Step ("");
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
		Debug.Log(string.Format("Chose {0} = {1}", name, value));
	}

	void Seg_OnNextEpisode ()
	{
		//TODO: get next episode;
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
		if (segment.IsHit(key)) {			
			HandleNextPosition (key.KeyName);
		} else {
			HandleMiss (key);
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
