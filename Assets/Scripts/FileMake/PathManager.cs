using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectData;

public class PathManager : MonoBehaviour {

	[SerializeField]
	private FileType type = FileType.Read;

	private List<string> address;
	private InputField input_f;
	private char[] separator = { '/', '\\' };
	private int view_index;
	//private bool flag = true;

	void Awake() {
		address = new List<string> ();

		string key = type == FileType.Read ? FileName.READ_PATH_KEY : FileName.WRITE_PATH_KEY;
		string line = PlayerPrefs.GetString (key, "C:/Users/Daiki/Desktop/卒研//解析ツール用/元データ");
		SetPath (line);
	}

	// Use this for initialization
	void Start () {
		input_f = this.GetComponent<InputField> ();
		//Debug.Log ("start()");
		input_f.text = GetPath (input_f.characterLimit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetPath(string path) {
		address.Clear();
		string[] tmp = path.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (string s in tmp) {
			address.Add (s);
		}
		//input_f.text = GetPath (input_f.characterLimit);
	}

	public void EndEdit() {
		//Debug.Log ("endedit");
		string buf = "";
		for (int i = 0; i < view_index; i++)
			buf += address [i] + "/";

		string[] tmp = input_f.text.Split (separator);
		for (int i = 1; i < tmp.Length; i++)
			buf += tmp [i] + "/";

		SetPath (buf);
	}

	public string GetPath() {
		string path = address[0];
		for (int i=1;i<address.Count;i++) {
			path += "/" + address [i];
		}
		return path;
	}

	public string GetPath(int limit) {
		if (address [address.Count - 1].Length > limit) {
			view_index = address.Count - 1;
			return ".../" + address [address.Count - 1];
		}
			
		int sum = address [address.Count - 1].Length;
		for (int i = address.Count - 2; i >= 1; i--) {
			sum += address [i].Length + 1;
			if (sum + 3 > limit) {
				view_index = i + 1;
				string buf = "...";
				for (int j = i + 1; j < address.Count; j++) {
					buf += "/" + address [j];
				}
				return buf;
			}
		}

		if (sum + address [0].Length <= limit) {
			view_index = 0;
			return GetPath ();
		} else {
			view_index = 1;
			string buf = "...";
			for (int i = 1; i < address.Count; i++)
				buf += "/" + address [i];
			return buf;
		}
	}

	public void GoBack() {
		address.RemoveAt (address.Count - 1);
		input_f.text = GetPath (input_f.characterLimit);
	}

	public void GoNext(string name) {
		address.Add (name);
		input_f.text = GetPath (input_f.characterLimit);
	}

	private enum FileType {
		Read,
		Write,
	}
}
