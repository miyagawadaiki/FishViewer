using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {

	public static void GoMain() {
		Debug.Log ("<color=green>Translate Scene : Main</color>");
		SceneManager.LoadScene ("Main");
	}

	public static void GoMaking() {
		Debug.Log ("<color=green>Translate Scene : Making</color>");
		SceneManager.LoadScene ("Making");
	}

	public static void GoFileMake() {
		Debug.Log ("<color=green>Translate Scene : FileMake</color>");
		SceneManager.LoadScene ("FileMake");
	}

	public static void GoFileRead() {
		Debug.Log ("<color=green>Translate Scene : FileRead</color>");
		SceneManager.LoadScene ("FileRead");
	}

	public static void GoStart() {
		Debug.Log ("<color=green>Translate Scene : Start</color>");
		SceneManager.LoadScene ("Start");
	}
}
