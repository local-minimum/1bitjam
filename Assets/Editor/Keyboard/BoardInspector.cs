using UnityEngine;
using UnityEditor;

namespace KeyBoard {
	[CustomEditor(typeof(Board))]
	public class BoardInspector : Editor {
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			EditorGUI.BeginChangeCheck ();
			var keyLayProp = serializedObject.FindProperty ("keyboardLayout");
			var newKeyboard = EditorGUILayout.IntSlider (keyLayProp.intValue, 0, 16);
			if (EditorGUI.EndChangeCheck()) {
				keyLayProp.intValue = newKeyboard;
				serializedObject.ApplyModifiedPropertiesWithoutUndo ();
				(target as Board).LoadLayout ();
			}
			if (GUILayout.Button ("Record layout")) {
				(target as Board).RecordLayout ();
			}

			EditorGUILayout.HelpBox (string.Format ("KeyBoard has {0} keys", (target as Board).keys.Length.ToString ()), MessageType.Info);
		}
	}

}