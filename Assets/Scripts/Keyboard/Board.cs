using UnityEngine;
using System.Collections.Generic;

namespace KeyBoard {
	public delegate void KeyPress(Key key);

	public class Board : MonoBehaviour {

		public event KeyPress OnKeyPress;

		[HideInInspector] public Key[] keys;
		public bool verboseKeys;

		[SerializeField, HideInInspector] int keyboardLayout = 0;

		Key recent;

		void Reset() {
			keys = GetComponentsInChildren<Key> ();
		}

		void OnEnable() {
			for (int i = 0; i < keys.Length; i++) {
				keys[i].OnKey += HandleKeyPress;
			}
		}

		void OnDisable() {
			for (int i = 0; i < keys.Length; i++) {
				keys[i].OnKey -= HandleKeyPress;
			}
		}

		void HandleKeyPress (Key key)
		{
			if (key.isDown && OnKeyPress != null)
				OnKeyPress (key);
		}

		public void RecordLayout() {
			for (int i = 0; i < keys.Length; i++) {
				keys [i].RecordKeyCode (keyboardLayout);
				keys [i].RecordPosition (keyboardLayout);
			}
		}

		public void LoadLayout() {
			for (int i = 0; i < keys.Length; i++) {
				keys [i].LoadLayout (keyboardLayout);
			}

		}

		public Key GetKey(string keyName) {
			keyName = keyName.ToUpper ();

			for (int i = 0; i < keys.Length; i++) {
				if (keys [i].KeyName == keyName)
					return keys [i];					
			}
			Debug.Log ("Requested unknown key: " + keyName);
			return null;
		}

		void Update() {
			if (verboseKeys && Input.anyKeyDown) {
				foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {

					if (Input.GetKeyDown (key)) {
						Debug.Log (key.ToString ());

						var current = GetKey (key.ToString());
						if (recent != null && current != null)
							Debug.Log (current.DistanceTo (recent));
						recent = current;
					}
				}
			}
		}
	}
}