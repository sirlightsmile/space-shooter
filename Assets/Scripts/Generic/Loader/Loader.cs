
using UnityEngine;

namespace SmileProject.Generic
{
	public static class Loader
	{
		public static string LoadTextFile(string filePath)
		{
			TextAsset targetFile = Resources.Load<TextAsset>(filePath);
			return targetFile.text;
		}
	}
}