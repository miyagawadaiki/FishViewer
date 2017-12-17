using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class FileReadManager : MonoBehaviour {

	[SerializeField]
	private Text n_path = null;
	[SerializeField]
	private Text n_name = null;
	[SerializeField]
	private FileMakeError fme = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Read() {
		if (System.IO.File.Exists (n_path.text + "/" + n_name.text) == false) {
			fme.ShowError ("ファイルが存在しません.\n正しいファイル名を記入してください.");
			Debug.Log ("Error2");
			return;
		}

		PlayerPrefs.SetString (PD::FileName.WRITE_PATH_KEY, n_path.text);
		PlayerPrefs.SetString (PD::FileName.WRITE_NAME_KEY, n_name.text);

		MySceneManager.GoMain ();
	}

	public void GoStart() {
		MySceneManager.GoStart ();
	}
}
