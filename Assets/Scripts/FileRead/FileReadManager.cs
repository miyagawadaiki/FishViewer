using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class FileReadManager : MonoBehaviour {

	[SerializeField]
	private PathManager n_path = null;
	[SerializeField]
	private InputField n_name = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Read() {
		PlayerPrefs.SetString (PD::FileName.WRITE_PATH_KEY, n_path.GetPath());
		PlayerPrefs.SetString (PD::FileName.WRITE_NAME_KEY, n_name.text);

		MySceneManager.GoMain ();
	}

	public void GoStart() {
		MySceneManager.GoStart ();
	}
}
