using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class SimuManager : MonoBehaviour {

	//[SerializeField]
	private List<GraphManager> graphs = new List<GraphManager>();
	[SerializeField]
	private Slider step_slider = null;
	[SerializeField]
	private BackgroundPanelController bpc = null;

	private bool playing = false, repeating = false, bpc_flag = false;
	private DataBase db;
	private StreamReader stream_r;
	private char[] split_words = { ',', ' ', '\t' };
	private float time = 0.0f;
	private bool flag1, flag2, flag3, flag4, flag5, flag6;

	[System.NonSerialized]
	public int step = 0, r_st_step = 0, r_go_step = 0;
	[System.NonSerialized]
	public float thre = 0.06f;

	void Awake() {
		// Debug
		Debug.Log("<color=green>Awake() in SimuManager</color>");

		string n_filename = PlayerPrefs.GetString (PD::FileName.WRITE_PATH_KEY, "") + "/" + PlayerPrefs.GetString (PD::FileName.WRITE_NAME_KEY, "");
		stream_r = new StreamReader (n_filename, Encoding.GetEncoding("UTF-8"));

		//Debug
		Debug.Log("n_filename = " + n_filename);

		string[] hoge = stream_r.ReadLine ().Split (split_words);
		PD::Parameter.STEPS = Int32.Parse(hoge[0]);
		PD::Parameter.FISH = Int32.Parse (hoge [1]);
		PD::Parameter.DELTA_TIME = float.Parse (hoge [2]);

		GameObject[] huga = GameObject.FindGameObjectsWithTag ("Graph");
		for (int i = 0; i < huga.Length; i++) {
			GraphManager[] foo = huga [i].GetComponents<GraphManager> ();
			foreach (GraphManager gm in foo) {
				graphs.Add (gm);
			}
		}
		
		db = DataBase.Instance;
		foreach(GraphManager g in graphs)
			g.db = DataBase.Instance;
		db.Init (PD::Parameter.STEPS);

		hoge = stream_r.ReadLine ().Split(split_words, StringSplitOptions.RemoveEmptyEntries);
		int type_num = hoge.Length / PD::Parameter.FISH;
		PD::Parameter.TYPES = new List<DataType> (type_num);
		for (int i = 0; i < type_num; i++) {
			PD::Parameter.TYPES.Add ((DataType)Enum.Parse (typeof(DataType), hoge [i]));
		}

		//Debug
		//Debug.Log("steps = " + PD::Parameter.STEPS);

		for (int i = 0; i < PD::Parameter.STEPS; i++) {
			
			string[] tmp = stream_r.ReadLine ().Split (split_words, StringSplitOptions.RemoveEmptyEntries);
			//Debug.Log (tmp.Length);

			for (int j = 0; j < PD::Parameter.FISH; j++) {
				for (int k = 0; k < PD::Parameter.TYPES.Count; k++) {
					db.SetData (j, PD::Parameter.TYPES[k], i, float.Parse(tmp[j * PD::Parameter.TYPES.Count + k]));
				}
			}
		}
			
	}

	// Use this for initialization
	void Start () {
		// Debug
		Debug.Log("<color=green>Start() in SimuManager</color>");

		step_slider.minValue = 0.0f;
		step_slider.maxValue = (float)PD::Parameter.STEPS;
		step_slider.value = 0.0f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		step = (int)(step_slider.value+0.5);

		time += Time.deltaTime;

		if (time < thre)
			return;

		time = 0.0f;

		if (playing) {
			GoNext ();
			//Debug
			//Debug.Log("step = " + step);
		} else {
			Step (step);
		}

		if (repeating) {
			if (step == r_go_step)
				Step (r_st_step);
		}

		if (Input.GetKey (KeyCode.Space) && !flag1) {
			SwitchBetweenPlayAndStop ();
			flag1 = true;
		}
		if (!Input.GetKey (KeyCode.Space) && flag1) {
			flag1 = false;
		}

		if (Input.GetKey (KeyCode.LeftArrow) && !flag2) {
			GoPrev ();
			flag2 = true;
		}
		if (!Input.GetKey (KeyCode.LeftArrow) && flag2) {
			flag2 = false;
		}

		if (Input.GetKey (KeyCode.RightArrow) && !flag3) {
			GoNext ();
			flag3 = true;
		}
		if (!Input.GetKey (KeyCode.RightArrow) && flag3) {
			flag3 = false;
		}

		if (Input.GetKey (KeyCode.R) && !flag4) {
			Reset ();
			flag4 = true;
		}
		if (!Input.GetKey (KeyCode.R) && flag4) {
			flag4 = false;
		}

		if (Input.GetKey (KeyCode.P) && !flag5) {
			SwitchRepeat ();
			flag5 = true;
		}
		if (!Input.GetKey (KeyCode.P) && flag5) {
			flag5 = false;
		}

		if (Input.GetKey (KeyCode.B) && !flag6) {
			SwitchBPC ();
			flag6 = true;
		}
		if (!Input.GetKey (KeyCode.B) && flag6) {
			flag6 = false;
		}
	}

	public bool IsPlaying() {
		return playing;
	}

	public void Play() {
		playing = true;
	}

	public void Stop() {
		playing = false;
	}

	public void SwitchBetweenPlayAndStop() {
		if (playing)
			Stop ();
		else
			Play ();
	}

	public void Reset() {
		Stop ();
		Step (0);
	}

	public void Step(int s) {
		if (s < 0 || IsEnd())
			return;
		
		step = s;

		for(int i = 0; i < graphs.Count; i++)
			graphs[i].Plot(step);

		step_slider.value = s;
	}

	public void GoNext() {
		Step (++step);
	}

	public void GoPrev() {
		Step (--step);
	}

	public bool IsRepeating() {
		return repeating;
	}

	public void ActivateRepeat() {
		repeating = true;
	}

	public void DeactivateRepeat() {
		repeating = false;
	}

	public void SwitchRepeat() {
		if (repeating)
			DeactivateRepeat ();
		else
			ActivateRepeat ();
	}

	public void SwitchBPC() {
		if (bpc_flag) {
			bpc.Deactivate ();
			bpc_flag = false;
		} else {
			bpc.Activate ();
			bpc_flag = true;
		}
	}

	public bool IsEnd() {
		return step >= PD::Parameter.STEPS;
	}

	public void GoStart() {
		MySceneManager.GoStart ();
	}
}
