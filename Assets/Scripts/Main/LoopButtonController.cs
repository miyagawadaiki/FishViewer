using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopButtonController : MonoBehaviour {

	[SerializeField]
	private SimuManager simu_m = null;
	[SerializeField]
	private Color enable_color;
	[SerializeField]
	private Color disable_color;
	[SerializeField]
	private GameObject start_obj = null;
	[SerializeField]
	private GameObject end_obj = null;

	private Image image;

	// Use this for initialization
	void Start () {
		image = this.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (simu_m.IsRepeating ()) {
			image.color = enable_color;
			start_obj.SetActive (true);
			end_obj.SetActive (true);
		} else {
			image.color = disable_color;
			start_obj.SetActive (false);
			end_obj.SetActive (false);
		}
	}
}
