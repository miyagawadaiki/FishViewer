using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class InputFieldInitializer : MonoBehaviour {

	[SerializeField]
	private BoxType type = BoxType.OldPath;

	private InputField input_f;
	private string key;

	// Use this for initialization
	void Start () {
		input_f = this.GetComponent<InputField> ();

		switch (type) {
		case BoxType.OldPath:
			key = PD::FileName.READ_PATH_KEY;
			break;
		case BoxType.OldName:
			key = PD::FileName.READ_NAME_KEY;
			break;
		case BoxType.NewPath:
			key = PD::FileName.WRITE_PATH_KEY;
			break;
		case BoxType.NewName:
			key = PD::FileName.WRITE_NAME_KEY;
			break;
		case BoxType.FishCount:
			input_f.text = PD::Parameter.FISH + "";
			return;
		case BoxType.DeltaTime:
			input_f.text = PD::Parameter.DELTA_TIME + "";
			return;
		}

		input_f.text = PlayerPrefs.GetString (key, "");
	}
	
	// Update is called once per frame
	void Update () {
		input_f.text = input_f.text.Replace("\\", "/");
	}
}

enum BoxType {
	OldPath,
	OldName,
	NewPath,
	NewName,
	FishCount,
	DeltaTime
}
