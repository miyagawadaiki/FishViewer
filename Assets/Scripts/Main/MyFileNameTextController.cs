using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class MyFileNameTextController : MonoBehaviour {

	private Text text;

	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text> ();

		string name = PlayerPrefs.GetString (PD::FileName.WRITE_NAME_KEY);

		text.text = name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
