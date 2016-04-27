using UnityEngine;
using UnityEngine.UI;

public class ForgroundColorPicker : MonoBehaviour {

	[SerializeField] Color32[] colors;
	[SerializeField] KeyCode[] keys;

	Image img;

	void Start () {
		img = GetComponent<Image> ();
	}
	

	void Update () {
		if (Input.anyKeyDown) {
			for (int i = 0; i < keys.Length; i++) {
				if (Input.GetKeyDown(keys[i]))
					img.color = colors[i];
			}
		}
	}
}
