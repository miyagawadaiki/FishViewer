using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayStopButtonController : MonoBehaviour {

	[SerializeField]
	private SimuManager simu_m = null;
	[SerializeField]
	private Sprite play_splite = null;
	[SerializeField]
	private Sprite stop_splite = null;

	private Image image;

	// Use this for initialization
	void Start () {
		image = this.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (simu_m.IsPlaying())
			image.sprite = stop_splite;
		else
			image.sprite = play_splite;
	}
}
