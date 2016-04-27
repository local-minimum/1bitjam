using UnityEngine;
using System.Collections.Generic;

public class StorySegmentChoice : StorySegment {
	
	[SerializeField] StorySegment nextSegment;
	[SerializeField] string text;
	int pos = -1;
	[SerializeField] string[] options;
	[SerializeField] string[] choices;
	List<string> activeOptions = new List<string> ();
	int optionPos = -1;
	int chosenOption = -1;
	[SerializeField] string optionName;

	public override bool Step (string keyName)
	{
		if (optionPos < 0)
			pos++;
		var keys = GetKeys (keyName);
		//Debug.Log (string.Join(", ", keys));
		if (keys.Length > 0) {
			SetKeys (keys);
			return true;
		} else {
			if (chosenOption != -1)
				ReportChoice (optionName, choices [chosenOption]);
			pos = -1;
			Progress (nextSegment);
			return false;
		}
	}

	public override void Restart ()
	{
		chosenOption = -1;
		optionPos = -1;
		pos = -1;
		Step ("");
	}

	string[] GetKeys(string keyName) {
		if (pos >= text.Length) {
			return new string[0];
		} else if (text [pos] == '$') {
			if (optionPos < 0) {
				activeOptions.AddRange (options);
			} else {
				var i = 0;
				while (i < activeOptions.Count) {

					if (activeOptions [i].Substring (optionPos, 1).ToUpper() == keyName) {
						i++;
					} else {
						activeOptions.RemoveAt (i);
					}
				}
			}
			optionPos++;
			for (int j = 0, l = activeOptions.Count; j < l; j++) {
				if (optionPos == activeOptions [j].Length) {
					chosenOption = System.Array.IndexOf (options, activeOptions [j]);
					optionPos = -1;
					pos++;
					return GetKeys (keyName);
				}
			}
			if (activeOptions.Count > 0) {
				var keys = new string[activeOptions.Count];
				for (int j = 0; j < keys.Length; j++)
					keys [j] = activeOptions [j].Substring (optionPos, 1);
				return keys;
			} else {
				optionPos = -1;
				pos++;
				return GetKeys (keyName);
			}
		} else {
			return new string[] { text.Substring (pos, 1) };
		}
	}
}
