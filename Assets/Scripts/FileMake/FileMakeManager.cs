using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class FileMakeManager : MonoBehaviour {

	[SerializeField]
	private Text o_path = null;
	[SerializeField]
	private Text o_name = null;
	[SerializeField]
	private Text n_path = null;
	[SerializeField]
	private Text n_name = null;
	[SerializeField]
	private Text fish_count = null;
	[SerializeField]
	private Text delta_time = null;
	[SerializeField]
	private FileMakeError fme = null;
	[SerializeField]
	private FileMakeError fme2 = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Register(bool flag) {
		if (!flag) {
			if (o_path.text == "" || o_name.text == "" || n_path.text == "" || n_name.text == ""
				|| fish_count.text == "" || delta_time.text == ""
			) {
				fme.ShowError ("空欄を埋めてください.");
				Debug.Log ("Error1");
				return;
			}

			if (System.IO.File.Exists (o_path.text + "/" + o_name.text) == false) {
				fme.ShowError ("変換前ファイルが存在しません.\n正しいファイル名を記入してください.");
				Debug.Log ("Error2");
				return;
			}

			if (System.IO.File.Exists (n_path.text + "/" + n_name.text) == true) {
				fme2.ShowError ("既に変換済みです.\n上書きしますか?");
				Debug.Log ("Error3");
				return;
			}
		}

		PlayerPrefs.SetString (PD::FileName.READ_PATH_KEY, o_path.text);
		PlayerPrefs.SetString (PD::FileName.READ_NAME_KEY, o_name.text);
		PlayerPrefs.SetString (PD::FileName.WRITE_PATH_KEY, n_path.text);
		PlayerPrefs.SetString (PD::FileName.WRITE_NAME_KEY, n_name.text);
		PlayerPrefs.SetString (PD::FileName.FISH_COUNT_KEY, fish_count.text);
		PlayerPrefs.SetString (PD::FileName.DELTA_TIME_KEY, delta_time.text);

		// Debug
		Debug.Log("<color=red>o_filename : " + o_path.text + "/" + o_name.text + "</color>");
		Debug.Log("<color=red>o_filename : " + n_path.text + "/" + n_name.text + "</color>");

		PD::Parameter.FISH = Int32.Parse(PlayerPrefs.GetString(PD::FileName.FISH_COUNT_KEY, "0"));
		PD::Parameter.DELTA_TIME = float.Parse (PlayerPrefs.GetString (PD::FileName.DELTA_TIME_KEY, "0.1"));

		MySceneManager.GoMaking ();
	}

	public void GoStart() {
		MySceneManager.GoStart ();
	}
}
