using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class LoopSliderController : MonoBehaviour {

	[SerializeField]
	private SimuManager simu_m = null;
	[SerializeField]
	private Slider end_slider = null;

	private Slider start_slider;

	// Use this for initialization
	void Start () {
		start_slider = this.GetComponent<Slider> ();
		start_slider.minValue = 0.0f;
		end_slider.minValue = 0.0f;
		start_slider.maxValue = (float)PD.Parameter.STEPS;
		end_slider.maxValue = (float)PD.Parameter.STEPS;
		start_slider.value = (float)simu_m.r_st_step;
		end_slider.value = (float)simu_m.r_go_step;
	}
	
	// Update is called once per frame
	void Update () {
		start_slider.value = (int)(start_slider.value + 0.5f);
		end_slider.value = (int)(end_slider.value + 0.5f);

		if (end_slider.value <= start_slider.value) {
			end_slider.value = start_slider.value;
		}

		simu_m.r_st_step = (int)start_slider.value;
		simu_m.r_go_step = (int)end_slider.value;
	}
}
