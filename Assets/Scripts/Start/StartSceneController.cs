using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoFileMake() {
		MySceneManager.GoFileMake ();
	}

	public void GoFileRead() {
		MySceneManager.GoFileRead ();
	}
}
