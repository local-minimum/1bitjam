using UnityEngine;
using System.Collections.Generic;

public class StorySegmentFork : StorySegment {
	[SerializeField] StorySegment[] nextSegment;

	int pos = -1;
	[SerializeField] string[] options;

	[SerializeField] string forkChoiceName;
	[SerializeField] string[] choices;

	List<string> activeOptions = new List<string> ();

	public override bool Step (string keyName)
	{
		if (pos < 0) {
			activeOptions.Clear ();
			activeOptions.AddRange(options);
		} else {			
			var i = 0;
			while (i < activeOptions.Count) {

				if (activeOptions [i].Substring (pos, 1).ToUpper() == keyName) {
					i++;
				} else {
					activeOptions.RemoveAt (i);
				}
			}
		}

		pos++;
		for (int j = 0, l = activeOptions.Count; j < l; j++) {
			if (pos == activeOptions [j].Length) {
				pos = -1;
				var choice = System.Array.IndexOf (options, activeOptions [j]);
				ReportChoice (forkChoiceName, choices [choice]);
				Progress (nextSegment [choice]);
				return false;
			}
		}
		SetKeys (GetKeys());
		return true;

	}

	string[] GetKeys() {
		string[] keys = new string[activeOptions.Count];
		for (int i = 0; i < keys.Length; i++)
			keys [i] = activeOptions [i].Substring (pos, 1);
		return keys;
	}

	public override void Restart ()
	{
		pos = -1;
		Step ("");
	}
}
