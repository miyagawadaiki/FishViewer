using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileMakeError : MonoBehaviour {

	[SerializeField]
	private Text text = null;

	// Use this for initialization
	void Start () {
		HideError ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowError(string err) {
		this.gameObject.SetActive (true);
		text.text = err;
	}

	public void HideError() {
		text.text = "";
		this.gameObject.SetActive (false);
	}
}
