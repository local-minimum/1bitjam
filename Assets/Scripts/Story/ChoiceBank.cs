using UnityEngine;
using System.Collections.Generic;

public class ChoiceBank : MonoBehaviour {

	[SerializeField] string baseUrl;

	Dictionary<string, string> bank = new Dictionary<string, string>();
	Dictionary<string, int> transactions = new Dictionary<string, int>();

	public void RegisterChoice(int episode, string key, string value) {
		transactions [key] = episode;
		bank [key] = value;
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
				choices.Add(string.Join("=", new string[] {key, bank[key]}));
			}
		}

		var geturl = string.Join ("&", choices.ToArray());
		return string.Join ("?", new string[] {baseUrl, geturl });
	}

	public void OpenEpisodePage(int episode) {
		Application.OpenURL (GetDisplayChoicesURL (episode));
	}
}
