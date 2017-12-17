using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class ReadingTextManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Text> ().text = 
			"Reading now : " + PlayerPrefs.GetString (PD::FileName.READ_NAME_KEY, "b") + ".csv";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
