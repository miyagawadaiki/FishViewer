using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExRateSliderController : MonoBehaviour {

	[SerializeField]
	private GameObject[] graph_objs = null;
	[SerializeField]
	private RateType type = RateType.X;
	[SerializeField]
	private Text text = null;

	private List<GraphManager> graphs = new List<GraphManager>();
	private Slider slider;

	// Use this for initialization
	void Start () {
		foreach (GameObject obj in graph_objs) {
			foreach (GraphManager gm in obj.GetComponents<GraphManager>()) {
				graphs.Add (gm);
			}
		}
		slider = this.GetComponent<Slider> ();
		slider.minValue = 1f;
		slider.maxValue = 5f;
		slider.value = 1f;
	}

	// Update is called once per frame
	void Update () {
		text.text = slider.value + "";
	}

	public void Expand() {
		if (type == RateType.X)
			for(int i = 0; i < graphs.Count; i++) graphs[i].Expand(slider.value, 0f);
		else
			for(int i = 0; i < graphs.Count; i++) graphs[i].Expand(0f, slider.value);
	}

	public void Reset() {
		slider.value = 1f;
		
	}

	private enum RateType {
		X, Y
	}
}
