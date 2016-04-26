using UnityEngine;
using System.Collections;

public class DistanceEmitter : MonoBehaviour {

	[SerializeField] ActiveSegment segment;
	[SerializeField, Range(1, 4)] float distanceScaling = 2f;
	[SerializeField, Range(1, 4)] int blinkOnRegisters = 2;

	Flasher flasher;

	void Start() {
		flasher = GetComponentInChildren<Flasher> ();
	}
		
	void OnEnable() {
		segment.OnMiss += Segment_OnMiss;
	}

	void OnDisable() {
		segment.OnMiss -= Segment_OnMiss;
	}

	void Segment_OnMiss (float distance)
	{	
			
		if (distance > 0) {
			int d = Mathf.RoundToInt (Mathf.Pow (distance + 1, distanceScaling));
			bool[] sequence = new bool[d + blinkOnRegisters];
			for (int i = d; i < sequence.Length; i++)
				sequence [i] = true;
			flasher.SetSequence (sequence);
		}
	}
}
