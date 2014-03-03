using UnityEngine;
using System.Collections;

public class GUIState
{
		private static ModelType mModelType = ModelType.None;

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
}
