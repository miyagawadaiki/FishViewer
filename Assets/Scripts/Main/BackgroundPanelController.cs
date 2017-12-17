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
		id_dd.ClearOptions ();
		for (int i = 0; i < PD::Parameter.FISH; i++)
			id_dd.options.Add (new Dropdown.OptionData ("ID : " + (i+1)));
		type_dd.ClearOptions ();
		foreach (string name in Enum.GetNames(typeof(DataType)))
			type_dd.options.Add (new Dropdown.OptionData (name));
		
		bc = bg.GetComponent<BackgroundController> ();
		toggle.isOn = bc.active;
		id_dd.value = 1; id_dd.value = bc.id;
		type_dd.value = 1; type_dd.value = (int)bc.type;
		slider.value = bc.threshold;

		db = DataBase.Instance;
		slider.minValue = db.GetMin (bc.type);
		slider.maxValue = db.GetMax (bc.type);
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
