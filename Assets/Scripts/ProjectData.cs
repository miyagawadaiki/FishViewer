using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectData {
	public class FileName {
		public static string READ_PATH_KEY = "OLD_PATH";
		public static string READ_NAME_KEY = "OLD_NAME";
		public static string WRITE_PATH_KEY = "NEW_PATH";
		public static string WRITE_NAME_KEY = "NEW_NAME";
		public static string FISH_COUNT_KEY = "FISH_COUNT";
		public static string DELTA_TIME_KEY = "DELTA_TIME";
		public static string DATA_TAGS_KEY = "DATA_TAGS";
		public static string BACKGROUND_GRAPH_KEY = "BACKGROUND_GRAPH";
		public static string BOTTOM_GRAPH_KEY = "BOTTOM_GRAPH_KEY";
		public static string RIGHT_GRAPH_KEY = "RIGHT_GRAPH_KEY";
	}

	public class Parameter {
		public static int STEPS = 10;
		public static int FISH = 5;
		public static int FISH_MAX = 5;
		public static float DELTA_TIME = 0.1f;
		public static float TANK_R = 740f;
		//public static int DATA_NUM = 15;
		public static List<DataType> TYPES = new List<DataType>();

		public static string[] GetNames() {
			string[] ret = new string[TYPES.Count];
			for (int i = 0; i < ret.Length; i++) {
				ret [i] = Enum.GetName (typeof(DataType), TYPES [i]);
			}

			return ret;
		}

		public static string[] GetTags() {
			string[] ret = new string[TYPES.Count];
			for (int i = 0; i < ret.Length; i++) {
				ret [i] = DataTypeEx.GetTag (TYPES [i]);
			}

			return ret;
		}

		public static bool Contains(DataType type) {
			for (int i = 0; i < TYPES.Count; i++) {
				if((int)TYPES[i] == (int)type) {
					return true;
				}
			}
			return false;
		}
	}
}