using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDragController : MonoBehaviour {

	[SerializeField]
	private Camera convert_camera = null;
	[SerializeField]
	private GameObject panel = null;

	private List<GraphManager> graphs;
	private Vector3 p_start, p_now, p_now_, e_start, e_now, e_now_, w_start;
	private RectTransform rect_t;
	private Text text;

	[System.NonSerialized]
	public bool active = true;

	// Use this for initialization
	void Start () {
		// Debug
		Debug.Log("<color=green>Start() in MouseDragController</color>");

		graphs = new List<GraphManager> ();
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag ("Graph")) {
			foreach(GraphManager g in obj.GetComponents<GraphManager>()) {
				graphs.Add(g);
			}
		}

		rect_t = panel.GetComponent<RectTransform> ();
		text = panel.GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!active)
			return;

		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			foreach (GraphManager graph in graphs) {
				string s = graph.GetSampleValue ((Vector2)convert_camera.ScreenToWorldPoint (Input.mousePosition));
				if (s != "") {
					text.text = s;
					panel.SetActive (true);
					rect_t.position = new Vector3 (Input.mousePosition.x + rect_t.rect.width / 2f, Input.mousePosition.y + rect_t.rect.height / 2f, 0f);
				}
			}
		} else {
			panel.SetActive (false);
		}

		if (Input.GetMouseButtonDown (0)) {
			p_start = convert_camera.ScreenToWorldPoint (Input.mousePosition);
			// Debug
			//Debug.Log("Click : " + camera.ScreenToWorldPoint (Input.mousePosition));
			p_now = p_start;
		}
		if (Input.GetMouseButton (0)) {
			p_now_ = convert_camera.ScreenToWorldPoint (Input.mousePosition);

			foreach(GraphManager graph in graphs) {
				if(graph.IsInArea(p_start))
					graph.Translate ((Vector2)(p_now - p_now_));
			}

			p_now = p_now_;
		}

		if (Input.GetMouseButtonDown (1)) {
			e_start = convert_camera.ScreenToWorldPoint (Input.mousePosition);
			// Debug
			//Debug.Log("Click : " + camera.ScreenToWorldPoint (Input.mousePosition));
			e_now = e_start;
		}
		if (Input.GetMouseButton (1)) {
			e_now_ = convert_camera.ScreenToWorldPoint (Input.mousePosition);

			foreach(GraphManager graph in graphs) {
				if (graph.IsInArea (e_start)) {
					graph.AddExpand((e_now_ - e_now).x * 0.5f, (e_now_ - e_now).y * 0.5f);
				}
			}

			e_now = e_now_;
		}

		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if (wheel != 0) {
			w_start = convert_camera.ScreenToWorldPoint (Input.mousePosition);

			// Debug
			Debug.Log("wheel : " + (int)(wheel*10) + " pos : " + (Vector2)w_start);

			foreach(GraphManager graph in graphs) {
				if (graph.IsInArea (w_start))
					graph.AddSample((int)(wheel*100));
			}
		}
	}
}
