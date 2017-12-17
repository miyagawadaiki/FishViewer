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
	}

	public class Parameter {
		public static int STEPS = 10;
		public static int FISH = 5;
		public static int FISH_MAX = 5;
		public static float DELTA_TIME = 0.1f;
		public static float TANK_R = 740f;
	}
}