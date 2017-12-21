using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FolderViewController : MonoBehaviour {

	[SerializeField]
	private GameObject content = null;
	[SerializeField]
	private GameObject node_obj = null;
	[SerializeField]
	private PathManager path_m = null;
	[SerializeField]
	private InputField file_if = null;

	private string file_name;
	private bool flag = true;

	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			UpdateView ();
			flag = false;
		}
	}

	public void UpdateView() {
		foreach (Transform transform in content.transform)
			Destroy (transform.gameObject);

		char[] separator = { '/', '\\' };

		string[] folders = Directory.GetDirectories (path_m.GetPath ());
		foreach (string name in folders) {
			string[] tmp = name.Split (separator);
			string s = tmp [tmp.Length - 1];
			GameObject obj = Instantiate (node_obj, content.transform);
			FileNode fn = obj.GetComponent<FileNode> ();
			fn.Set (true, s);
			fn.path_m = this.path_m;
			fn.fvc = this;
			fn.input_f = file_if;
		}

		string[] files = Directory.GetFiles (path_m.GetPath (), "*.csv");
		foreach (string name in files) {
			string[] tmp = name.Split (separator);
			string s = tmp [tmp.Length - 1];
			GameObject obj = Instantiate (node_obj, content.transform);
			FileNode fn = obj.GetComponent<FileNode> ();
			fn.Set (false, s);
			fn.path_m = this.path_m;
			fn.fvc = this;
			fn.input_f = file_if;
		}
	}

	public void CallForUpdateView() {
		flag = true;
	}
}
