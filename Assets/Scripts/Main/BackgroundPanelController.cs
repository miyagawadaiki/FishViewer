using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class BackgroundPanelController : MonoBehaviour {

	[SerializeField]
	private Toggle toggle = null;
	[SerializeField]
	private Dropdown id_dd = null;
	[SerializeField]
	private Dropdown type_dd = null;
	[SerializeField]
	private Slider slider = null;
	[SerializeField]
	private Text min_text = null;
	[SerializeField]
	private Text max_text = null;
	[SerializeField]
	private Text thre_text = null;
	[SerializeField]
	private MouseDragController mdc = null;
	[SerializeField]
	private GameObject bg = null;

	private BackgroundController bc;
	private DataBase db;

	// Use this for initialization
	void Start () {
		db = DataBase.Instance;

		id_dd.ClearOptions ();
		for (int i = 0; i < PD::Parameter.FISH; i++)
			id_dd.options.Add (new Dropdown.OptionData ("ID : " + (i+1)));
		type_dd.ClearOptions ();
		foreach (string name in PD::Parameter.GetNames())
			type_dd.options.Add (new Dropdown.OptionData (name));
		
		bc = bg.GetComponent<BackgroundController> ();

		//PlayerPrefs.DeleteKey (PD::FileName.BACKGROUND_GRAPH_KEY);
		string line = PlayerPrefs.GetString (PD::FileName.BACKGROUND_GRAPH_KEY, "0,0,6,10");
		// Debug
		//Debug.Log("<color=blue>line = " + line + "</color>");
		char[] separator = { ',' };
		string[] tmp = line.Split (separator, StringSplitOptions.RemoveEmptyEntries);

		int id = Int32.Parse(tmp[1]) < PD::Parameter.FISH ? Int32.Parse(tmp[1]) : 0;
		DataType t = PD::Parameter.Contains ((DataType)Int32.Parse (tmp [2])) ? (DataType)Int32.Parse (tmp [2]) : DataType.Distance;
		float th = (db.GetMin(t) + db.GetMax(t)) / 2;

		toggle.isOn = Int32.Parse(tmp[0]) > 0 ? true : false;
		id_dd.value = 1; id_dd.value = id;
		type_dd.value = 1; type_dd.value = (int)t;
		slider.minValue = db.GetMin (t);
		slider.maxValue = db.GetMax (t);
		slider.value = th;
		UpdateParameter ();

		//slider.minValue = db.GetMin (bc.type);
		//slider.maxValue = db.GetMax (bc.type);
		min_text.text = db.GetMin (bc.type) + "";
		max_text.text = db.GetMax (bc.type) + "";
		thre_text.text = "Threshold : " + (float)((int)(slider.value * 100) / 100f);

		Deactivate ();
	}
	
	// Update is called once per frame
	void Update () {
		slider.minValue = db.GetMin ((DataType)type_dd.value);
		slider.maxValue = db.GetMax ((DataType)type_dd.value);
		min_text.text = db.GetMin ((DataType)type_dd.value) + "";
		max_text.text = db.GetMax ((DataType)type_dd.value) + "";
		thre_text.text = "Threshold : " + slider.value;
	}

	public void UpdateParameter() {
		bc.active = toggle.isOn;
		bc.id = id_dd.value;
		bc.type = (DataType)type_dd.value;
		bc.threshold = slider.value;

		string s = (toggle.isOn ? "1" : "0") + ",";
		s += bc.id + ",";
		s += ((int)bc.type) + ",";
		s += bc.threshold;
		PlayerPrefs.SetString (PD::FileName.BACKGROUND_GRAPH_KEY, s);

		Deactivate ();
	}

	public void Activate() {
		this.gameObject.SetActive(true);
		mdc.active = false;

		toggle.isOn = bc.active;
		id_dd.value = bc.id;
		type_dd.value = (int)bc.type;
		slider.value = bc.threshold;
	}

	public void Deactivate() {
		mdc.active = true;
		this.gameObject.SetActive (false);
	}
}
