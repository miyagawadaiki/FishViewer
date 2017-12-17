using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSliderController : MonoBehaviour {

	[SerializeField]
	private SimuManager simu_m = null;
	[SerializeField]
	private Text text = null;
	[SerializeField]
	private float default_speed;

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = this.GetComponent<Slider> ();
		slider.minValue = 0.0f;
		slider.maxValue = 4.0f;
		slider.value = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		simu_m.thre = default_speed / slider.value;
		text.text = "x" + slider.value;
	}
}
