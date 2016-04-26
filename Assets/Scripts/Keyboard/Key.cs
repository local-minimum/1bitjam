using UnityEngine;
using System.Collections;

namespace KeyBoard {

	public enum KeyEventTypes {Resting, Down, Held, Up};

	public delegate void KeyEvent(Key key);

	public class Key : MonoBehaviour {

		public event KeyEvent OnKey;

		public KeyCode keyCode;
		string _keyName;

		[SerializeField] int layout = 0;
		[SerializeField] KeyCode[] layouts = new KeyCode[0];
		[SerializeField] Vector3[] positions = new Vector3[0];

		KeyEventTypes _eventType = KeyEventTypes.Resting;

		public bool isResting {
			get {
				return _eventType == KeyEventTypes.Resting;
			}
		}

		public bool isDown {
			get {
				return _eventType == KeyEventTypes.Down;
			}
		}

		public bool isHeld {
			get {
				return _eventType == KeyEventTypes.Down || _eventType == KeyEventTypes.Held;
			}
		}

		public bool isUp {
			get {
				return _eventType == KeyEventTypes.Up;
			}
		}

		public string KeyName {
			get {
				return _keyName;
			}
		}

		void Start() {
			LoadLayout (layout);
		}

		void Update() {
			KeyEventTypes prevState = _eventType;

			if (Input.GetKeyUp (keyCode)) {
				_eventType = KeyEventTypes.Up;
			} else if (Input.GetKeyDown (keyCode)) {
				_eventType = KeyEventTypes.Down;
			} else if (Input.GetKey (keyCode)) {
				_eventType = KeyEventTypes.Held;
			} else {
				_eventType = KeyEventTypes.Resting;
			}

			if (prevState != _eventType) {
				if (OnKey != null) {
					OnKey (this);
				}
			}
		}

		void OnDrawGizmos() {

			Color color;
			float upScale;
			if (isDown) {
				color = Color.blue;
				upScale = 0.4f;
			} else if (isHeld) {
				color = Color.cyan;
				upScale = 0.25f;
			} else if (isUp) {
				color = Color.green;
				upScale = 0.5f;
			} else {
				color = Color.gray;
				upScale = 0.7f;
			}

			Gizmos.color = color;
			Gizmos.DrawWireCube (transform.position, transform.TransformVector(new Vector3(1, 1, upScale)));
		}

		public void LoadLayout(int layout) {
			if (layouts.Length > layout)
				keyCode = layouts [layout];
			else
				keyCode = KeyCode.None;

			if (positions.Length > layout) {
				transform.localPosition = positions [layout];
			}

			if (keyCode == KeyCode.Space)
				_keyName = " ";
			else if (keyCode != KeyCode.None)
				_keyName = keyCode.ToString ();
			else
				_keyName = "";

		}

		public void RecordKeyCode(int layout) {
			if (layout >= layouts.Length) {
				var newLayouts = new KeyCode [layout + 1];
				System.Array.Copy (layouts, newLayouts, layouts.Length);
				newLayouts [layout] = keyCode;
				layouts = newLayouts;
			} else {
				layouts [layout] = keyCode;
			}

			if (keyCode != KeyCode.None)				
				_keyName = keyCode.ToString ();
			else
				_keyName = "";
		}

		public void RecordPosition(int layout) {
			if (layout >= positions.Length) {
				var newPositions = new Vector3 [layout + 1];
				System.Array.Copy (positions, newPositions, positions.Length);
				newPositions [layout] = transform.localPosition;
				positions = newPositions;
			} else {
				positions [layout] = transform.localPosition;
			}
		}

		public void RecordCustomLayout() {
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown (key)) {
					if (layout >= layouts.Length) {
						var newLayouts = new KeyCode [layout + 1];
						System.Array.Copy (layouts, newLayouts, layouts.Length);
						newLayouts [layout] = key;
						layouts = newLayouts;
						return;
					}
				}
			}
		}

		public float DistanceTo(Key other) {
			
			float x, y, z;
			float d = Vector3.Distance (transform.position, other.transform.position);
			for (int i = 0; i < 6; i++) {
				x = (i % 2) * 2 - 1;
				y = (i % 4 > 1) ? 1 : -1;
				z = (i > 3) ? 1 : -1;
				if (d < 0)
					d = Vector3.Distance (transform.TransformPoint (new Vector3 (x, y, z)), other.transform.TransformPoint (new Vector3 (x, y, z)));
				else {
					d = Mathf.Min (d, Vector3.Distance (transform.TransformPoint (new Vector3 (x, y, z)), other.transform.TransformPoint (new Vector3 (x, y, z))));
				}
			}
			return d;
		}
	}

}
