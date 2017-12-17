using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSliderController : MonoBehaviour {

	[SerializeField]
	private GraphManager[] graphs = null;
	[SerializeField]
	private Text text = null;

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = this.GetComponent<Slider> ();
		slider.minValue = 1f;
		slider.maxValue = 100f;
		slider.value = 50f;
	}
	
	// Update is called once per frame
	void Update () {
		slider.value = (int)(slider.value + 0.5f);
		text.text = (int)slider.value + "";
	}

	public void UpdateSample() {
		foreach (GraphManager graph in graphs) {
			graph.UpdateSample ((int)slider.value);
		}
	}

	public void Reset() {
		slider.value = 50f;
	}
}
