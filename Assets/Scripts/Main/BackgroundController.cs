using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	[SerializeField]
	private SimuManager simu_m = null;

	[System.NonSerialized]
	public bool active;
	[System.NonSerialized]
	public int id;
	[System.NonSerialized]
	public DataType type;
	[System.NonSerialized]
	public float threshold;

	private Color st_color = Color.black;
	private Color sp_color = new Color(29f / 255f, 26f / 255f, 56f / 255f, 1f);
	private SpriteRenderer sr;
	private DataBase db;

	void Awake() {
		active = false;
		id = 0;
		type = DataType.Speed;
		threshold = 0f;
		sr = this.GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
		db = DataBase.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		float value = db.GetData (id, type, simu_m.step);
		if (active && value >= threshold) {
			sr.color = sp_color;
		} else {
			sr.color = st_color;
		}
	}
}
