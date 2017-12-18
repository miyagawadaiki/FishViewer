using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class BottomGraphController : MonoBehaviour {

	//[SerializeField]
	public Toggle toggle = null;
	[SerializeField]
	private Dropdown id_dd = null;
	[SerializeField]
	private Dropdown output_dd = null;
	[SerializeField]
	private Slider slider = null;
	[SerializeField]
	private Text text = null;

	//[System.NonSerialized]
	public GraphManager graph;

	// Use this for initialization
	void Start () {
		// Debug
		Debug.Log("<color=green>Start() in BottomGraphController</color>");

		id_dd.ClearOptions ();
		for (int i = 0; i < PD::Parameter.FISH; i++)
			id_dd.options.Add (new Dropdown.OptionData ("ID : " + (i+1) + ""));

		id_dd.value = 1;	id_dd.value = 0;

		output_dd.ClearOptions ();
		string[] names = PD::Parameter.GetNames();
		for (int i=(int)DataType.Distance;i<names.Length;i++)
			output_dd.options.Add (new Dropdown.OptionData(names[i]));

		output_dd.value = 1;	output_dd.value = 0;

		slider.minValue = 1f;
		slider.maxValue = 5f;
		slider.value = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = slider.value + "";
		graph.Expand (0f, slider.value);
	}

	public void UpdateGraph() {
		if (toggle.isOn)
			graph.Enable ();
		else
			graph.Disable ();

		graph.fish_id = id_dd.value;

		graph.output_type = (DataType)(output_dd.value + (int)DataType.Distance);
		graph.ResetViewParameter ();

		graph.Expand (0f, slider.value);
	}

	public void Load() {
		id_dd.value = graph.fish_id;
		output_dd.value = (int)graph.output_type - (int)DataType.Distance;
		slider.value = graph.y_ex_rate;
	}
}
