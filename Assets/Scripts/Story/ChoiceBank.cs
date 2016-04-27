using UnityEngine;
using System.Collections.Generic;

public class ChoiceBank : MonoBehaviour {

	[SerializeField] string baseUrl;
	[SerializeField] string[] episodeUrls;
	Dictionary<string, string> bank = new Dictionary<string, string>();
	Dictionary<string, int> transactions = new Dictionary<string, int>();

	static ChoiceBank _instance;

	public static string GetChoiceMade(string key) {
		if (_instance)
			return _instance.GetChoice(key);
		return "";
	}

	void Start() {
		_instance = this;
	}

	void OnDestroy() {
		_instance = null;
	}

	public void RegisterChoice(int episode, string key, string value) {
		transactions [key] = episode;
		bank [key] = value;
	}

	public void Wipe() {
		bank.Clear ();
		transactions.Clear ();
	}

	public string GetChoice(string key) {
		if (bank.ContainsKey (key))
			return bank [key];
		Debug.LogWarning (string.Format ("Choice {0} requested but not yet made", key));
		return "";
	}

	string GetDisplayChoicesURL(int episode) {
		List<string> choices = new List<string>();

		foreach (var key in transactions.Keys) {
			if (transactions[key] == episode) {
				choices.Add(string.Join("=", new string[] {key , WWW.EscapeURL(bank[key])}));
			}
		}

		var geturl = string.Join ("&", choices.ToArray());
		return string.Join ("?", new string[] {baseUrl + episodeUrls[episode], geturl });
	}

	public void OpenEpisodePage(int episode) {
		Debug.Log ("Showing choices for episode " + episode);
		Application.OpenURL (GetDisplayChoicesURL (episode));
	}
}
