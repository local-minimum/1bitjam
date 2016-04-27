using UnityEngine;
using System.Collections;

public class BackgroundColorPicker : MonoBehaviour {

	[SerializeField] Color32[] colors;
	[SerializeField] KeyCode[] keys;

	Camera cam;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			for (int i = 0; i < keys.Length; i++) {
				if (Input.GetKeyDown (keys [i]))
					cam.backgroundColor = colors [i];
			}
		}
	}
}
