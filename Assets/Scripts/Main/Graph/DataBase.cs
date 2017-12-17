using System;
using System.Collections;
using System.Collections.Generic;

using PD = ProjectData;

public class DataBase {

	private static DataBase mInstance;
	private float[,,] data;
	private float[] max, min;
	private int length;

	private DataBase() {

	}

	public static DataBase Instance {
		get {
			if (mInstance == null)
				mInstance = new DataBase ();
			return mInstance;
		}
	}

	public void Init(int len) {
		data = new float[PD.Parameter.FISH, Enum.GetNames(typeof(DataType)).Length, len];
		max = new float[Enum.GetNames(typeof(DataType)).Length];
		min = new float[Enum.GetNames(typeof(DataType)).Length];
		for (int i = 0; i < max.Length; i++) {
			min [i] = 1000f;
			max [i] = -1000f;
		}
		length = len;
	}

	public void SetData(int fish_id, DataType type, int index, float value) {
		data [fish_id, (int)type, index] = value;
		if (value > max [(int)type])
			max [(int)type] = value;
		if (value < min [(int)type])
			min [(int)type] = value;
	}

	public float GetData(int fish_id, DataType type, int index) {
		if (index < 0 || index >= length)
			return 0;
		return data [fish_id, (int)type, index];
	}

	public float GetMax(DataType type) {
		return max [(int)type];
	}

	public float GetMin(DataType type) {
		return min [(int)type];
	}
}
