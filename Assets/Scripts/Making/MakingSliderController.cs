using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class MakingSliderController : MonoBehaviour {

	private FileMaker file_maker;
	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = this.GetComponent<Slider> ();
		slider.maxValue = (float)PD::Parameter.STEPS;
		slider.minValue = 0f;
		slider.value = 0f;

		file_maker = GameObject.Find("FileMaker").GetComponent<FileMaker>();
	}
	
	// Update is called once per frame
	void Update () {
		slider.value = file_maker.Index;
	}
}
