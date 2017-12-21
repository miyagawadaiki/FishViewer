using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileNode : MonoBehaviour {

	[SerializeField]
	private GameObject file = null;
	[SerializeField]
	private GameObject folder = null;
	[SerializeField]
	private Text text = null;

	private bool is_folder = false;

	[System.NonSerialized]
	public PathManager path_m;
	[System.NonSerialized]
	public FolderViewController fvc;
	[System.NonSerialized]
	public InputField input_f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set(bool b, string name) {
		is_folder = b;
		text.text = name;

		if (is_folder) {
			// Debug
			//Debug.Log ("This is a folder");
			file.SetActive (false);
			folder.SetActive (true);
		} else {
			folder.SetActive (false);
			file.SetActive (true);
		}
	}

	public bool IsFolder() {
		return is_folder;
	}

	public string GetName() {
		return text.text;
	}

	public void Execute() {
		if (is_folder) {
			path_m.GoNext (GetName ());
			fvc.CallForUpdateView ();
		} else {
			input_f.text = GetName ();
		}
	}
}
