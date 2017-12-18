using System;
using System.Collections;
using System.Collections.Generic;

using ProjectData;

public enum DataType {
	PositionX,
	PositionY,
	VelocityX,
	VelocityY,
	AccelerationX,
	AccelerationY,
	Distance,
	Speed,
	Acceleration,
	Angle,
	ClosestDist,
	ClosestDistAve,
	WallValue,
	FishValue,
	SchoolValue,
	TimeDist,
}

public class DataTypeEx {

	public static int Count() {
		return Enum.GetNames (typeof(DataType)).Length;
	}

	public static string[] GetTags() {
		string[] ret = new string[Enum.GetNames(typeof(DataType)).Length];
		for (int i = 0; i < ret.Length; i++) {
			ret [i] = GetTag ((DataType)i);
		}

		return ret;
	}

	public static string GetTag(DataType type) {
		switch (type) {
		case DataType.PositionX:
			return "位置(x)";
		case DataType.PositionY:
			return "位置(y)";
		case DataType.VelocityX:
			return "速度(x)";
		case DataType.VelocityY:
			return "速度(y)";
		case DataType.AccelerationX:
			return "加速度(x)";
		case DataType.AccelerationY:
			return "加速度(y)";
		case DataType.Distance:
			return "中心からの距離";
		case DataType.Speed:
			return "速さ";
		case DataType.Acceleration:
			return "加速度の大きさと正負";
		case DataType.Angle:
			return "偏角";
		case DataType.ClosestDist:
			return "最近傍魚との距離";
		case DataType.ClosestDistAve:
			return "最近傍魚との距離の履歴的平均値";
		case DataType.WallValue:
			return "壁による魚への刺激の度合";
		case DataType.FishValue:
			return "群れから受ける刺激の度合";
		case DataType.SchoolValue:
			return "群れに加わっている度合";
		case DataType.TimeDist:
			return "時間距離";
		default:
			return "";
		}
	}

}