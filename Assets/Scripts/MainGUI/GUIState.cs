using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIState
{
		private static ModelType mModelType = ModelType.None;
		private static FunctionType mFunctionType = FunctionType.None;

		public static Dictionary<ModelType, string> ModelTypes = new Dictionary<ModelType, string> ()
	{
		{ ModelType.None, "<-- Select -->" },
		{ ModelType.NewYorkSkyline, "Empire State 3D" },
		{ ModelType.Vegas, "Vegas 3D" }
	};

		public static Dictionary<FunctionType, string> EmpireFunctionTypes = new Dictionary<FunctionType, string> ()
	{
		{ FunctionType.None, "<-- Select -->" },
		{ FunctionType.Panoramic, "Panoramic" },
		{ FunctionType.Model3D, "3D Model" }
	};

		public static Dictionary<FunctionType, string> VegasFunctionTypes = new Dictionary<FunctionType, string> ()
	{
		{ FunctionType.None, "<-- Select -->" },
		{ FunctionType.Panoramic, "Panoramic" },
		{ FunctionType.Model3D, "3D Model" }
	};

		public enum FunctionType
		{
				None = 0,
				Panoramic = 1,
				Model3D = 2
		}

		public enum ModelType
		{
				None = 0,
				NewYorkSkyline = 1,
				Vegas = 2
		}

		public static ModelType mSelectedModelType {
				get {
						return mModelType;
				}

				set {
						mModelType = value;
				}
		}

		public static FunctionType mSelectedFunctionType {
				get {
						return mFunctionType;
				}

				set {
						mFunctionType = value;
				}
		}
}
