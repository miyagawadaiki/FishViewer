using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class RightGraphsManager : MonoBehaviour {

	[SerializeField]
	private GraphManager[] graphs = null;
	[SerializeField]
	private Toggle[] toggles = null;
	[SerializeField]
	private Dropdown input_dd = null;
	[SerializeField]
	private Dropdown output_dd = null;
	[SerializeField]
	private MouseDragController mdc = null;

	private Rect rect;

	// Use this for initialization
	void Start () {
		// Debug
		Debug.Log("<color=green>Start() in RightGraphsManager</color>");

		//PlayerPrefs.DeleteKey (PD::FileName.RIGHT_GRAPH_KEY);
		int tmp = PlayerPrefs.GetInt (PD::FileName.RIGHT_GRAPH_KEY, 708);
		// Debug
		Debug.Log("<color=blue>tmp = " + tmp + "</color>");
		foreach (GraphManager graph in graphs) {
			graph.input_type = (DataType)(tmp/100);
			graph.output_type = (DataType)(tmp%100);
		}

		input_dd.ClearOptions();
		output_dd.ClearOptions();

		foreach (String name in PD::Parameter.GetNames()) {
			input_dd.options.Add (new Dropdown.OptionData (name));
			output_dd.options.Add (new Dropdown.OptionData (name));
		}

		input_dd.value = (int)graphs[0].input_type;
		output_dd.value = (int)graphs[0].output_type;

		for (int i = 0; i < PD::Parameter.FISH_MAX; i++) {
			if (i < PD::Parameter.FISH) {
				toggles [i].isOn = true;
				graphs [i].Enable ();
			} else {
				toggles [i].gameObject.SetActive (false);
				graphs [i].Disable ();
			}
		}

		rect = this.GetComponent<RectTransform> ().rect;
		//Debug.Log ("rect : " + rect);
		//Debug.Log ("rect.size : " + rect.size);
		Vector2 vec = GameObject.Find ("Canvas").GetComponent<RectTransform> ().localScale;
		Vector2 center = rect.center;
		rect.size = new Vector2 (rect.size.x * vec.x, rect.size.y * vec.y);
		rect.center = center;

		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		// Debug
		//Debug.Log("Cursor is in Panel? : " + rect.Contains(Input.mousePosition - this.transform.position));
		//Debug.Log("Canvas Scaler : " + GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution);

		if (Input.GetMouseButtonDown (0) && !rect.Contains(Input.mousePosition - this.transform.position)) {
			Deactivate ();
		}
	}

	public void UpdateDataType() {
		foreach (GraphManager graph in graphs) {
			graph.input_type = (DataType) input_dd.value;
			graph.output_type = (DataType)output_dd.value;
			graph.ResetViewParameter ();
		}

		for (int i = 0; i < toggles.Length; i++) {
			if (toggles [i].isOn) {
				graphs [i].Enable ();
			} else {
				graphs [i].Disable ();
			}
		}

		PlayerPrefs.SetInt (PD::FileName.RIGHT_GRAPH_KEY, input_dd.value * 100 + output_dd.value);

		Deactivate ();
	}

	public void Activate() {
		this.gameObject.SetActive(true);
		mdc.active = false;

		input_dd.value = (int)graphs[0].input_type;
		output_dd.value = (int)graphs[0].output_type;
	}
		
	public void Deactivate() {
		mdc.active = true;
		this.gameObject.SetActive (false);
	}

	public void Reset() {
		foreach (GraphManager graph in graphs) {
			graph.ResetViewParameter ();
		}
	}
}
