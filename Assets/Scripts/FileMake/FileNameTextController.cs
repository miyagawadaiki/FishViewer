using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileNameTextController : MonoBehaviour {

	[SerializeField]
	private InputField path_if = null;
	[SerializeField]
	private InputField name_if = null;

	private Text text;

	public Text Text {
		get { return text; }
	}

	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = path_if.text;
		if (name_if.text != "")
			text.text += "/" + name_if.text;
	}
}
