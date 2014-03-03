using UnityEngine;
using System.Collections;

public class GUIState
{
		public enum ModelType
		{
				NewYorkSkyline,
				Vegas
		}

		public static ModelType mSelectedModelType { get; set; }
}
