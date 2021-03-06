﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomGraphsManager : MonoBehaviour {
	
	[SerializeField]
	private GameObject graph_obj = null;
	[SerializeField]
	private MouseDragController mdc = null;
	[SerializeField]
	private BottomGraphController[] bgcs = null;

	private GraphManager[] graphs;
	private Gradient g;
	private Rect rect;
	private Color[] colors = {
		new Color(255f / 255f,0f,0f,255f / 255f),
		new Color(255f / 255f,54f / 255f,198f / 255f,255f / 255f),
		new Color(255f / 255f,237f / 255f,0f,255f / 255f),
		new Color(0f,255f / 255f,33f / 255f,255f / 255f),
		new Color(30f / 255f,0f,255f / 255f,255f / 255f),
		new Color(255f / 255f,153f / 255f,0f,255f / 255f)
	};

	// Use this for initialization
	void Start () {
		// Debug
		Debug.Log("<color=green>Start() in BottomGraphsManager</color>");

		graphs = graph_obj.GetComponents<GraphManager> ();

		for (int i = 0; i < graphs.Length; i++) {
			// Debug
			//Debug.Log("colors[" + i + "] : " + colors[i]);

			g = new Gradient();
			GradientColorKey[] gck = new GradientColorKey[2];
			gck [0].color = Color.white;	gck [0].time = 0.0f;
			gck [1].color = colors[i];		gck [1].time = 1.0f;
			GradientAlphaKey[] gak = new GradientAlphaKey[2];
			gak [0].alpha = 0.0f;		gak [0].time = 0.0f;
			gak [1].alpha = 0xfff;		gak [1].time = 1.0f;
			g.SetKeys (gck, gak);

			graphs[i].top_color = colors [i];
			graphs[i].bottom_color = g.Evaluate(0.55f);
			graphs [i].Init ();

			bgcs [i].graph = graphs [i];
		}

		rect = this.GetComponent<RectTransform> ().rect;
		//Debug.Log ("rect : " + rect);
		//Debug.Log ("rect.size : " + rect.size);
		Vector2 vec = GameObject.Find ("Canvas").GetComponent<RectTransform> ().localScale;
		Vector2 center = rect.center;
		rect.size = new Vector2 (rect.size.x * vec.x, rect.size.y * vec.y);
		rect.center = center;

		bgcs [0].graph.Enable (); bgcs [0].graph.output_type = DataType.Speed;
		bgcs [1].graph.Enable (); bgcs [1].graph.output_type = DataType.FishValue;
		bgcs [2].graph.Disable ();
		bgcs [3].graph.Disable ();
		bgcs [4].graph.Disable ();

		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !rect.Contains(Input.mousePosition - this.transform.position)) {
			//Deactivate ();
		}
	}

	public void UpdataGraphs() {
		for (int i = 0; i < graphs.Length; i++) {
			bgcs [i].UpdateGraph ();
		}

		Deactivate ();
	}

	public void Activate() {
		this.gameObject.SetActive(true);
		mdc.active = false;

		for (int i = 0; i < graphs.Length; i++) {
			bgcs [i].Load ();
		}
	}

	public void Deactivate() {
		mdc.active = true;
		this.gameObject.SetActive (false);
	}
}
