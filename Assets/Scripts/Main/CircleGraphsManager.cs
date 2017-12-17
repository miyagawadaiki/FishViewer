using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PD = ProjectData;

public class CircleGraphsManager : MonoBehaviour {

	private GraphManager[] graphs;

	// Use this for initialization
	void Start () {
		graphs = this.GetComponents<GraphManager> ();
		for (int i = 0; i < PD::Parameter.FISH_MAX; i++) {
			if(i < PD::Parameter.FISH)
				graphs [i].Enable ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
