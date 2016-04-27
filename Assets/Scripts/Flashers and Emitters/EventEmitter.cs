using UnityEngine;
using System.Collections;

public class EventEmitter : MonoBehaviour {

	[SerializeField] ActiveSegment segment;

	[SerializeField] bool[] FoundKeySequence;
	[SerializeField] bool[] WaitingSequence;
	[SerializeField] bool[] SegmentComplete;
	[SerializeField] bool[] ResetSegment;
	[SerializeField] bool[] MenuSequence;
	[SerializeField] bool[] EndSequence;

	Flasher flasher;

	void Start() {
		flasher = GetComponentInChildren<Flasher> ();
	}

	void OnEnable() {
		segment.OnFoundKey += Segment_OnFoundKey;
		segment.OnRestartSegment += Segment_OnRestartSegment;
		segment.OnCompletedSegment += Segment_OnCompletedSegment;
		segment.OnCompletedEpisode += Segment_OnCompletedEpisode;
		segment.OnCompletedGame += Segment_OnCompletedGame;
		segment.OnEpisodeStarted += Segment_OnEpisodeStarted;
	}

	void OnDisable() {
		segment.OnFoundKey -= Segment_OnFoundKey;
		segment.OnRestartSegment -= Segment_OnRestartSegment;
		segment.OnCompletedSegment -= Segment_OnCompletedSegment;
		segment.OnCompletedEpisode -= Segment_OnCompletedEpisode;
		segment.OnCompletedGame -= Segment_OnCompletedGame;
		segment.OnEpisodeStarted -= Segment_OnEpisodeStarted;
	}

	void Segment_OnEpisodeStarted ()
	{
		flasher.SetSequence (WaitingSequence);
	}
		
	void Segment_OnCompletedGame ()
	{
		flasher.SetSequence (EndSequence, MenuSequence);
	}

	void Segment_OnCompletedEpisode ()
	{
		flasher.SetSequence (SegmentComplete, MenuSequence);
	}

	void Segment_OnCompletedSegment ()
	{
		flasher.SetSequence (SegmentComplete, WaitingSequence);
	}

	void Segment_OnRestartSegment ()
	{
		flasher.SetSequence (ResetSegment, MenuSequence);
	}

	void Segment_OnFoundKey ()
	{		
		flasher.SetSequence (FoundKeySequence, WaitingSequence);
	}
}
