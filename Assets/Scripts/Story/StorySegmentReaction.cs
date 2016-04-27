using UnityEngine;
using System.Collections.Generic;

public class StorySegmentReaction : StorySegment {
	[SerializeField] StorySegment[] nextSegment;

	int pos = -1;
	[SerializeField] string[] reactions;

	[SerializeField] string reactionKeyName;
	[SerializeField] string[] choices;
	[SerializeField] int defaultReaction = 0;

	string activeReaction;

	public override bool Step (string keyName)
	{
		if (pos < 0) {
			var choice = ChoiceBank.GetChoiceMade (reactionKeyName);
			var index = System.Array.IndexOf (choices, choice);
			if (index >= 0) {
				activeReaction = reactions [index];
				Debug.Log("Playing: " + activeReaction);
			} else {
				Debug.LogWarning ("Choice '" + choice + "' unknown, using default");
				activeReaction = reactions [defaultReaction];
			}
		}
		pos++;
		if (pos < activeReaction.Length) {
			SetKeys (new string[] { activeReaction.Substring (pos, 1)});
			return true;
		} else {
	
			var choice = System.Array.IndexOf (reactions, activeReaction);
			Progress (nextSegment [choice]);
			return false;
		}

	}

	public override void Restart ()
	{
		pos = -1;
		Step ("");
	}

}
