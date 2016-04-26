using UnityEngine;
using System.Collections;

public class EventEmitter : MonoBehaviour {

	[SerializeField] ActiveSegment segment;

	[SerializeField] bool[] FoundKeySequence;
	[SerializeField] bool[] WaitingSequence;
	[SerializeField] bool[] SegmentComplete;
	[SerializeField] bool[] ResetSegment;

	Flasher flasher;

	void Start() {
		flasher = GetComponentInChildren<Flasher> ();
	}

	void OnEnable() {
		segment.OnFoundKey += Segment_OnFoundKey;
		segment.OnRestartSegment += Segment_OnRestartSegment;
		segment.OnCompletedSegment += Segment_OnCompletedSegment;
	}

	void OnDisable() {
		segment.OnFoundKey -= Segment_OnFoundKey;
		segment.OnRestartSegment -= Segment_OnRestartSegment;
		segment.OnCompletedSegment -= Segment_OnCompletedSegment;
	}

	void Segment_OnCompletedSegment ()
	{
		flasher.SetSequence (SegmentComplete, WaitingSequence);
	}

	void Segment_OnRestartSegment ()
	{
		flasher.SetSequence (ResetSegment, WaitingSequence);
	}

	void Segment_OnFoundKey ()
	{		
		flasher.SetSequence (FoundKeySequence, WaitingSequence);
	}
}
