using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.Text;
using UnityEngine;
using UnityEngine.UI;

using PD = ProjectData;

public class FileMaker : MonoBehaviour {

	[SerializeField]
	private GameObject node_obj = null;
	[SerializeField]
	private GameObject content_obj = null;

	private Toggle[] toggles;
	private Text[] texts;

	private StreamReader stream_r;
	private StreamWriter stream_w;
	private char[] split_words = { ',', ' ', '\t' };
	private int fish = PD::Parameter.FISH;
	private float dt = PD::Parameter.DELTA_TIME;
	private int index = 3;
	private int count;

	private Vector2[] pos, n_pos, n_n_pos;
	private Vector2[] vel, n_vel;
	private Vector2[] acc;
	private float[] sp, p_sp;
	private float[,] acd, afa;
	private int ave_num = 50;

	private bool flag = false;

	public int Index {
		get { return index; }
	}

	void Awake() {
		toggles = new Toggle[DataTypeEx.Count()];
		texts = new Text[DataTypeEx.Count()];

		for (int i = 0; i < DataTypeEx.Count (); i++) {
			GameObject obj = Instantiate (node_obj, content_obj.transform) as GameObject;
			toggles [i] = obj.GetComponent<Toggle> ();
			toggles [i].isOn = false;
			texts [i] = obj.GetComponentInChildren<Text> ();
			texts [i].text = DataTypeEx.GetTag ((DataType)i);
		}

		string line = PlayerPrefs.GetString (PD::FileName.DATA_TAGS_KEY, "None");
		if (line == "None") {
			for (int i = 0; i < DataTypeEx.Count (); i++) {
				//PD::Parameter.TYPES.Add ((DataType)i);
				toggles [i].isOn = true;
			}
		} else {
			char[] separator = {',', ' ', '\n', '\r'};
			string[] tmp = line.Split (separator, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < tmp.Length; i++) {
				DataType dt = (DataType)Enum.Parse (typeof(DataType), tmp [i]);
				//PD::Parameter.TYPES.Add (dt);
				toggles [(int)dt].isOn = true;
			}
		}


	}

	void Start() {

	}

	// Update is called once per frame
	void Update () {

	}

	public void UpdateTypes() {
		PD::Parameter.TYPES = new List<DataType>();

		string buf = "";
		for (int i = 0; i < DataTypeEx.Count(); i++) {
			if (toggles [i].isOn) {
				PD::Parameter.TYPES.Add ((DataType)i);
				buf += Enum.GetName(typeof(DataType), (DataType)i) + ",";
			}
		}

		PlayerPrefs.SetString (PD::FileName.DATA_TAGS_KEY, buf);
	}

	// Use this for initialization
	public void Make () {
		
		UpdateTypes ();

		string o_filename = PlayerPrefs.GetString (PD::FileName.READ_PATH_KEY, "") + "/" + PlayerPrefs.GetString (PD::FileName.READ_NAME_KEY, "");
		string n_filename = PlayerPrefs.GetString (PD::FileName.WRITE_PATH_KEY, "") + "/" + PlayerPrefs.GetString (PD::FileName.WRITE_NAME_KEY, "");
		stream_r = new StreamReader (o_filename, Encoding.GetEncoding("UTF-8"));
		stream_w = new StreamWriter (n_filename, false, Encoding.GetEncoding("UTF-8"));

		stream_r.ReadLine ();
		count = 0;
		while (stream_r.ReadLine() != null) {
			count++;
		}
		PD::Parameter.STEPS = count - 2;
		// Log
		Debug.Log (count);

		stream_r.Close ();

		stream_w.WriteLine (PD::Parameter.STEPS + "," + PD::Parameter.FISH + "," + PD::Parameter.DELTA_TIME);
		string tags = "";
		for (int i = 0; i < fish; i++) {
			foreach(string name in PD::Parameter.GetNames()) {
			//foreach (string name in Enum.GetNames(typeof(DataType))) {
				tags += name + ",";
			}
			tags += ",";
		}
		stream_w.WriteLine (tags);

		stream_r = new StreamReader (o_filename, Encoding.GetEncoding("UTF-8"));
		//stream_r.ReadLine ();

		//for (int i = 0; i < fish; i++) {
		pos = new Vector2[fish];
		n_pos = new Vector2[fish];
		n_n_pos = new Vector2[fish];
		vel = new Vector2[fish];
		n_vel = new Vector2[fish];
		acc = new Vector2[fish];
		sp = new float[fish];
		p_sp = new float[fish];
		acd = new float[fish, ave_num];
		afa = new float[fish, ave_num];
		//}
			
		string line = stream_r.ReadLine ();
		string[] tmp = line.Split (split_words, StringSplitOptions.RemoveEmptyEntries);
		/*if (tmp.Length < fish * 2) {
			PD::Parameter.FISH = tmp.Length / 2;
			fish = tmp.Length / 2;
		}*/

		for (int i = 0; i < fish; i++) {
			pos [i] = new Vector2 (float.Parse (tmp [i * 2]), float.Parse (tmp [i * 2 + 1]));
		}

		line = stream_r.ReadLine ();
		tmp = line.Split (split_words, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < fish; i++) {
			n_pos [i] = new Vector2 (float.Parse (tmp [i * 2]), float.Parse (tmp [i * 2 + 1]));
			vel [i] = (new Vector2 (n_pos[i].x - pos[i].x, n_pos[i].y - pos[i].y)) / dt;
			p_sp [i] = vel [i].magnitude;
		}

		while (Step ()) ;

		MySceneManager.GoMain ();
	}

	public bool Step() {
		if (count < index) {
			if (!flag) {
				flag = true;
				stream_r.Close ();
				stream_w.Close ();
				MySceneManager.GoMain ();
			}
			return false;
		} else {

			string line = stream_r.ReadLine ();

			string[] tmp = line.Split (split_words, StringSplitOptions.RemoveEmptyEntries);

			string write_buf = "";

			for (int i = 0; i < fish; i++) {
				// posX, posY, velX, velY, accX, accY
				n_n_pos [i] = new Vector2 (float.Parse (tmp [i * 2]), float.Parse (tmp [i * 2 + 1]));
				n_vel [i] = (new Vector2 (n_n_pos [i].x - n_pos [i].x, n_n_pos [i].y - n_pos [i].y)) / dt;
				acc [i] = (new Vector2 (n_vel [i].x - vel [i].x, n_vel [i].y - vel [i].y)) / dt;
				//Debug.Log ("acc : " + acc [i].ToString ());

				// 絶対にデータに含むため省略
				write_buf += pos [i].x + "," + pos [i].y + "," + vel [i].x + "," + vel [i].y + "," + acc [i].x + "," + acc [i].y + ",";



				// dist
				float dist = pos[i].magnitude;

				if(PD::Parameter.Contains(DataType.Distance))
					write_buf += dist + ",";



				// speed
				float abs_vel = vel[i].magnitude;
					//float abs_acc = Mathf.Sqrt (acc [i].x * acc [i].x + acc [i].y * acc [i].y);
				float n_abs_vel = n_vel[i].magnitude;

				if(PD::Parameter.Contains(DataType.Speed))
					write_buf += abs_vel + ",";

					//write_buf += abs_acc + ",";

				// acceleration
				sp[i] = abs_vel;

				if(PD::Parameter.Contains(DataType.Acceleration))
					write_buf += ((sp [i] - p_sp [i]) / PD::Parameter.DELTA_TIME) + ",";
					//write_buf += ((abs_vel < n_abs_vel) ? abs_acc : -abs_acc) + ",";

					// acceleration or deceleration
					//write_buf += ((abs_vel < n_abs_vel) ? (1) : (0)) + ",";



				// argument angle
				float cross_product = vel [i].x * n_vel [i].y - vel [i].y * n_vel [i].x;
				float angle = CalcAngle (vel [i], n_vel [i]);
				//if (float.IsNaN (angle)) Debug.Log ("<color=red>angle = " + Mathf.Acos((vel [i].x * n_vel [i].x + vel [i].y * n_vel [i].y) / (abs_vel * n_abs_vel)) + "</color>");
				//if (float.IsNaN (angle)) Debug.Log ("<color=red>vel[i] = " + vel [i] + ", n_vel[i] = " + n_vel [i] + "</color>");
				//float angle = Mathf.Acos ((vel [i].x * n_vel [i].x + vel [i].y * n_vel [i].y) / (abs_vel * n_abs_vel));

				if(PD::Parameter.Contains(DataType.Angle))
					write_buf += ((cross_product > 0) ? (angle) : (-angle)) + ",";



				// closest dist
				float min_dist = 10000f;
				int closest_index = 0;
				for (int j = 0; j < fish; j++) {
					if (i == j)
						continue;
					float f = (pos [i] - pos [j]).magnitude;
					if (f < min_dist) {
						min_dist = f;
						closest_index = j;
					}
				}
				//min_dist = (pos[i] - pos[closest_index]).magnitude;

				if(PD::Parameter.Contains(DataType.ClosestDist))
					write_buf += min_dist + ",";



				// c-dist average
				acd [i, (index-2) % ave_num] = min_dist;
				float c_dist_sum = 0f;
				for (int j = 0; j < ave_num; j++) {
					c_dist_sum += acd [i, j];
				}
					
				float c_dist_ave = c_dist_sum / ((index < ave_num) ? index-2 : ave_num);

				if(PD::Parameter.Contains(DataType.ClosestDistAve))
					write_buf += c_dist_ave + ",";



				// wall value
				if(PD::Parameter.Contains(DataType.WallValue))
					write_buf += (dist / PD::Parameter.TANK_R) + ",";



				// fish value
				float fish_value = (2 * PD::Parameter.TANK_R - c_dist_ave) / (2 * PD::Parameter.TANK_R);

				if(PD::Parameter.Contains(DataType.FishValue))
					write_buf += fish_value + ",";



				// school value
				afa[i, (index-2) % ave_num] = Mathf.PI - CalcAngle (vel [i], vel[closest_index]);
				float f_ang_sum = 0f;
				for (int j = 0; j < ave_num; j++) {
					f_ang_sum += afa [i, j];
				}
				float school_value = fish_value * 0.7f + f_ang_sum / ave_num / Mathf.PI * 0.3f;

				if(PD::Parameter.Contains(DataType.SchoolValue))
					write_buf += school_value + ",";



				// time distance
				float time_dist;
				if (sp [i] == 0f || min_dist / sp [i] > 170.088f)
					time_dist = 170.088f;
				else
					time_dist = min_dist / sp [i];

				if(PD::Parameter.Contains(DataType.TimeDist))
					write_buf += time_dist + ",";

				pos [i] = n_pos [i];
				n_pos [i] = n_n_pos [i];
				vel [i] = n_vel [i];
				p_sp [i] = sp [i];

				write_buf += ",";
			}

			stream_w.WriteLine (write_buf);

			index++;
		}

		return true;
	}

	public void GoFileMake() {
		MySceneManager.GoFileMake ();
	}

	private float CalcAngle(Vector2 a, Vector2 b) {
		float cos = (a.x * b.x + a.y * b.y) / (a.magnitude * b.magnitude);
		if (cos > 1f)
			cos = 1f;
		else if (cos < -1f)
			cos = -1f;
		return Mathf.Acos (cos);
	}
}