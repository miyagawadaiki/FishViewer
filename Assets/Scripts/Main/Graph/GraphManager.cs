using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {

	[SerializeField]
	private bool my_enabled = false;
	public int fish_id = 0;
	[SerializeField]
	private int def_sample_num = 50;
	[SerializeField]
	private AlignType align = AlignType.Right;
	[SerializeField]
	private bool use_step = false;

	public DataType input_type = DataType.PositionX;

	public DataType output_type = DataType.Speed;
	[SerializeField][Range(0f, 1f)]
	private float def_scale = 0f;

	public Color top_color = new Color(0f,0f,0f,0xfff);

	public Color bottom_color = new Color(0f,0f,0f,0xfff);
	[SerializeField]
	private bool use_color_gradient = false;
	[SerializeField]
	private bool use_scale_gradient = false;
	[SerializeField]
	private bool try_centering_x = false;
	[SerializeField]
	private bool try_centering_y = false;
	[SerializeField]
	private bool show_border = false;
	[SerializeField]
	private bool show_axis = false;
	[SerializeField]
	private bool can_translate = true;
	[SerializeField]
	private bool can_expand = true;
	[SerializeField]
	private Material line_material = null;

	private int marker;
	private float[] input_array, output_array;
	private Vector2 graph_pos;
	private float width, height;
	private float X_MIN, X_MAX, Y_MIN, Y_MAX;
	private GameObject[] samples;
	private LineRenderer x_axis, y_axis;
	private float ox, oy;
	private int sample_max = 300, sample_num;
	private Gradient g;
	private GameObject obj;

	[System.NonSerialized]
	public DataBase db;
	[System.NonSerialized]
	public float x_ex_rate = 1f, y_ex_rate = 1f;

	void Awake() {
		// Debug
		Debug.Log("<color=green>Awake() in GraphManager</color>");

		Init ();
	}

	// Use this for initialization
	void Start () {
		// Debug
		Debug.Log("<color=green>Start() in GraphManager</color>");

		db = DataBase.Instance;

		if (!use_step) {
			X_MIN = db.GetMin(input_type);
			X_MAX = db.GetMax (input_type);
		}

		Y_MIN = db.GetMin (output_type);
		Y_MAX = db.GetMax (output_type);

		if (X_MIN > 0 && X_MIN < 10)
			X_MIN = -0.5f;
		if (X_MAX < 0 && X_MAX > -10)
			X_MAX = 0.5f;
		if (Y_MIN > 0 && Y_MIN < 10)
			Y_MIN = -0.5f;
		if (Y_MAX < 0 && Y_MAX > -10)
			Y_MAX = 0.5f;

		if (try_centering_x) {
			X_MIN = -1f * Mathf.Max (Mathf.Abs(X_MIN), Mathf.Abs(X_MAX));
			X_MAX = -1 * X_MIN;
		}
		if (try_centering_y) {
			Y_MIN = -1f * Mathf.Max (Mathf.Abs(Y_MIN), Mathf.Abs(Y_MAX));
			Y_MAX = -1 * Y_MIN;
		}

		// Debug
		//Debug.Log("(X_MIN, X_MAX) = " + new Vector2(X_MIN, X_MAX) + " in " + input_type);

		/*
		// サンプルオブジェクトの大きさを決める
		float scale = def_scale;
		if(use_step) {
			scale = width / (sample_num + 5);
			//marker_obj.transform.localScale = new Vector3 (scale, scale, 2f);
			//sample_obj.transform.localScale = new Vector3 (scale, scale, 2f);
		}
		*/


		// 軸の初期値設定
		ox = (X_MAX + X_MIN) / 2;
		oy = (Y_MAX + Y_MIN) / 2;
		if (X_MIN * X_MAX < 0f)
			ox = 0f;
		if (Y_MIN * Y_MAX < 0f)
			oy = 0f;
		if (use_step)
			ox = X_MIN + marker;
	}
	
	// Update is called once per frame
	void Update () {
		if (!my_enabled)
			return;
		ShowAxis ();
	}

	public void Init() {
		sample_num = def_sample_num;

		// マーカーの番号を決定
		if (align == AlignType.Left)
			marker = 0;
		else if (align == AlignType.Center)
			marker = sample_num / 2;
		else
			marker = sample_num - 1;

		// 座標変換用パラメータを設定
		graph_pos = this.transform.position;
		width = this.transform.localScale.x * this.transform.GetChild (0).localScale.x;
		height = this.transform.localScale.y * this.transform.GetChild (0).localScale.y;


		// データ配列の初期化
		input_array = new float[sample_max];
		output_array = new float[sample_max];

		// 時間変化グラフ用に入力データ配列を初期化
		for (int i = 0; i < sample_max; i++)
			input_array [i] = i;

		// サンプルのGameObjectを設定
		samples = new GameObject[sample_max];

		obj = Resources.Load ("sample") as GameObject;

		for(int i=0;i<sample_max;i++) {
			samples [i] = Instantiate (obj, new Vector3(graph_pos.x, graph_pos.y, 2f), Quaternion.identity);
			samples [i].transform.parent = this.transform;
		}

		// Gradientを設定
		g = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[2];
		gck [0].color = bottom_color;	gck [0].time = 0.0f;
		gck [1].color = top_color;	gck [1].time = 1.0f;
		GradientAlphaKey[] gak = new GradientAlphaKey[2];
		gak [0].alpha = 0.0f;	gak [0].time = 0.0f;
		gak [1].alpha = 0xfff;	gak [1].time = 1.0f;
		g.SetKeys (gck, gak);

		UpdateSample (def_sample_num);


		// 枠線を引く
		if (show_border) {
			LineRenderer border = this.GetComponent<LineRenderer> ();
			border.materials[0] = line_material;
			border.startWidth = 0.02f;
			border.positionCount = 4;
			border.SetPosition (0, (Vector3)graph_pos + new Vector3 (-width / 2f, -height / 2f, -0.1f));
			border.SetPosition (1, (Vector3)graph_pos + new Vector3 (width / 2f, -height / 2f, -0.1f));
			border.SetPosition (2, (Vector3)graph_pos + new Vector3 (width / 2f, height / 2f, -0.1f));
			border.SetPosition (3, (Vector3)graph_pos + new Vector3 (-width / 2f, height / 2f, -0.1f));
			border.loop = true;
		}

		// 軸を書く準備
		if (show_axis) {
			GameObject bar = new GameObject ("x_axis");
			bar.transform.parent = this.transform;
			bar.AddComponent<LineRenderer> ();
			x_axis = bar.GetComponent<LineRenderer> ();
			x_axis.materials[0] = line_material;
			x_axis.positionCount = 2;
			x_axis.startWidth = 0.02f;

			bar = new GameObject ("y_axis");
			bar.transform.parent = this.transform;
			bar.AddComponent<LineRenderer> ();
			y_axis = bar.GetComponent<LineRenderer> ();
			y_axis.materials [0] = line_material;
			y_axis.positionCount = 2;
			y_axis.startWidth = 0.02f;
		}
	}

	public void Plot(int step) {
		if (!my_enabled)
			return;

		if (use_step) {
			X_MAX = step + sample_num - marker;
			X_MIN = step - marker;
			ox = (X_MAX + X_MIN) / 2;
		}

		for (int i = 0; i < sample_num; i++) {

			// 入力が時間でないときはデータベースから取得する
			if (!use_step)
				input_array [i] = db.GetData (fish_id, input_type, step - marker + i);
			
			// 出力データを取得
			output_array [i] = db.GetData (fish_id, output_type, step - marker + i);

			// 座標の変換
			float x, x_, y, y_, z;
			x = input_array [i] + (use_step ? X_MIN : 0);
			y = output_array[i];
			x_ = x_ex_rate * (width - 0.5f) / (X_MAX - X_MIN) * (x - (X_MAX + X_MIN) / 2) + graph_pos.x;
			y_ = y_ex_rate * (height - 0.5f) / (Y_MAX - Y_MIN) * (y - (Y_MAX + Y_MIN) / 2) + graph_pos.y;
			if (float.IsNaN (y_))
				Debug.Log ("<color=red>(step, y, Ymax, Ymin) = (" + step + ", " + y + ", " + Y_MAX + ", " + Y_MIN + ")</color>");
			z = (IsInArea (new Vector2 (x_, y_)) && step - marker + i >= 0) ? -0.2f : 2f;
			samples [i].transform.position = new Vector3 (x_, y_, z);

		}
	}

	public void ShowAxis() {
		if (show_axis) {

			float ox_, oy_;
			ox_ = x_ex_rate * (width - 0.5f) / (X_MAX - X_MIN) * (ox - (X_MAX + X_MIN) / 2);
			oy_ = y_ex_rate * (height - 0.5f) / (Y_MAX - Y_MIN) * (oy - (Y_MAX + Y_MIN) / 2);

			if (IsInArea ((Vector2)graph_pos + new Vector2 (0f, oy_))) {
				x_axis.enabled = true;
				x_axis.SetPosition (0, (Vector3)graph_pos + new Vector3 (-width / 2, oy_, -0.1f));
				x_axis.SetPosition (1, (Vector3)graph_pos + new Vector3 (width / 2, oy_, -0.1f));
			} else {
				x_axis.enabled = false;
			}
			if (IsInArea ((Vector2)graph_pos + new Vector2 (ox_, 0f))) {
				y_axis.enabled = true;
				y_axis.SetPosition (0, (Vector3)graph_pos + new Vector3 (ox_, -height / 2, -0.1f));
				y_axis.SetPosition (1, (Vector3)graph_pos + new Vector3 (ox_, height / 2, -0.1f));
			} else {
				y_axis.enabled = false;
			}
		}
	}

	public void Translate(Vector2 vec) {
		if (!can_translate)
			return;
		
		Vector2 hoge = new Vector2 ();
		hoge.x = (X_MAX - X_MIN) / x_ex_rate / (width - 0.5f) * vec.x;
		hoge.y = (Y_MAX - Y_MIN) / y_ex_rate / (height - 0.5f) * vec.y;

		if (!use_step) {
			X_MIN += hoge.x;
			X_MAX += hoge.x;
		}
		Y_MIN += hoge.y;
		Y_MAX += hoge.y;
	}

	public void UpdateSample(int num) {
		if (num < 0 || num >= sample_max)
			return;
		
		sample_num = num;

		if (align == AlignType.Left)
			marker = 0;
		else if (align == AlignType.Center)
			marker = sample_num / 2;
		else
			marker = sample_num - 1;

		// サンプルオブジェクトの大きさを決める
		float scale = def_scale;
		if(use_step) {
			scale = width / (sample_num + 5);
			//marker_obj.transform.localScale = new Vector3 (scale, scale, 2f);
			//sample_obj.transform.localScale = new Vector3 (scale, scale, 2f);
		}

		int i;
		for (i = 0; i < sample_num; i++) {
			//samples [i] = Instantiate (obj, new Vector3(graph_pos.x, graph_pos.y, 2f), Quaternion.identity);
			if (i == marker) {
				samples [i].transform.localScale = new Vector3 (scale, scale, 1f);
				if (use_step)
					samples [i].transform.localScale = new Vector3 (scale * 3f, scale * 3f, 1f);
				samples [i].GetComponent<SpriteRenderer> ().color = top_color;
			}
			else if(use_color_gradient || use_scale_gradient) {
				if (i < marker) {
					float s = (float)(i + 10) / (marker + 10) * scale;
					if(use_scale_gradient) samples [i].transform.localScale = new Vector3 (s, s, 1);
					else samples [i].transform.localScale = new Vector3 (scale, scale, 1);
					if(use_color_gradient) samples [i].GetComponent<SpriteRenderer> ().color = g.Evaluate ((float)i / sample_num);
				} else {
					float s = (float)(sample_num-i-1+10) / (sample_num - marker - 1 + 10) * scale;
					if(use_scale_gradient) samples [i].transform.localScale = new Vector3 (s, s, 1);
					else samples [i].transform.localScale = new Vector3 (scale, scale, 1);
					if(use_color_gradient) samples [i].GetComponent<SpriteRenderer> ().color = g.Evaluate ((float)(sample_num - i - 1) / sample_num);
				}
			}
			else {
				samples [i].transform.localScale = new Vector3 (scale, scale, 1);
				samples [i].GetComponent<SpriteRenderer> ().color = bottom_color;
			}
		}

		for (; i < sample_max; i++) {
			samples [i].transform.position += new Vector3 (0f, 0f, 10f);
		}
	}

	public void AddSample(int a) {
		if (sample_num + a == 0)
			UpdateSample (1);
		else UpdateSample (sample_num + a);
	}

	public void Expand(float x, float y) {
		if (!can_expand)
			return;

		if(x > 0 && !use_step)
			x_ex_rate = x;
		if(y > 0)
			y_ex_rate = y;
	}

	public void AddExpand(float dx, float dy) {
		Expand (x_ex_rate + dx, y_ex_rate + dy);
	}

	public void ResetViewParameter() {
		if (!use_step) {
			X_MIN = db.GetMin(input_type);
			X_MAX = db.GetMax (input_type);
		}

		Y_MIN = db.GetMin (output_type);
		Y_MAX = db.GetMax (output_type);

		if (X_MIN > 0 && X_MIN < 10)
			X_MIN = -0.5f;
		if (X_MAX < 0 && X_MAX > -10)
			X_MAX = 0.5f;
		if (Y_MIN > 0 && Y_MIN < 10)
			Y_MIN = -0.5f;
		if (Y_MAX < 0 && Y_MAX > -10)
			Y_MAX = 0.5f;

		if (Y_MIN >= 0f && Y_MAX <= 1f) {
			Y_MIN = 0f;
			Y_MAX = 1f;
		}
			

		if (try_centering_x) {
			X_MIN = -1f * Mathf.Max (Mathf.Abs(X_MIN), Mathf.Abs(X_MAX));
			X_MAX = -1 * X_MIN;
		}
		if (try_centering_y) {
			Y_MIN = -1f * Mathf.Max (Mathf.Abs(Y_MIN), Mathf.Abs(Y_MAX));
			Y_MAX = -1 * Y_MIN;
		}

		x_ex_rate = 1f;
		y_ex_rate = 1f;
		sample_num = def_sample_num;

		UpdateSample (sample_num);
	}

	public bool IsInArea(Vector2 vec) {
		return (vec.x >= graph_pos.x - width / 2f) && (vec.x <= graph_pos.x + width / 2f) && (vec.y >= graph_pos.y - height / 2f) && (vec.y <= graph_pos.y + height / 2f);
	}

	public void Enable() {
		my_enabled = true;
	}

	public void Disable() {
		my_enabled = false;

		foreach (GameObject obj in samples)
			obj.transform.position += new Vector3 (0f, 0f, 10f);
	}

	public string GetSampleValue(Vector2 vec) {
		for (int i=0;i<sample_num;i++) {
			GameObject obj = samples [i];
			Vector2 pos = (Vector2)obj.transform.position;
			float scale = obj.transform.localScale.x;
			if((pos - vec).sqrMagnitude <= scale * scale) {
				return ToString(i);
			}
		}
		return "";
	}

	public string ToString(int index) {
		string ret = "";
		Vector2 vec = (Vector2)samples [index].transform.position;
		Vector2 hoge = new Vector2 ();
		hoge.x = (X_MAX - X_MIN) / x_ex_rate / (width - 0.5f) * (vec.x - this.transform.position.x) + (X_MAX + X_MIN) / 2f;
		hoge.y = (Y_MAX - Y_MIN) / y_ex_rate / (height - 0.5f) * (vec.y - this.transform.position.y) + (Y_MAX + Y_MIN) / 2f;
		if (!use_step) {
			ret += index + ", " + hoge.x + ", ";
		} else {
			ret += (index + X_MIN) + ", ";
		}
		ret += hoge.y;

		return ret;
	}

	private enum AlignType {
		Left, Center, Right
	}
}
