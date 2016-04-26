using UnityEngine;
using System.Collections;

public class StorySegmentLinear : StorySegment {

	[SerializeField] StorySegment nextSegment;
	[SerializeField] string text;
	int pos = -1;

	public override bool Step (string keyName)
	{
		pos++;
		if (pos < text.Length) {
			SetKeys (text.Substring(pos, 1));
			return true;
		} else {
			Progress (nextSegment);
			return false;
		}
	}

	public override void Restart ()
	{
		pos = 0;
		SetKeys (text.Substring (pos, 1));
	}
}
