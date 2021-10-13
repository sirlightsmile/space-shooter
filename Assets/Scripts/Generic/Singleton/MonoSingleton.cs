
using UnityEngine;

namespace SmileProject.Generic
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance;

		public static T GetInstance()
		{
			if (instance == null)
			{
				GameObject instanceObj = new GameObject($"{typeof(T).Name}_Instance");
				instance = instanceObj.AddComponent<T>();
				DontDestroyOnLoad(instanceObj);
			}
			return instance;
		}

		protected virtual void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(this);
				throw new System.Exception("An instance of this singleton already exists.");
			}
			else
			{
				instance = this.GetComponent<T>();
			}
		}

		private void OnDestroy()
		{
			instance = null;
		}

		private void OnApplicationQuit()
		{
			instance = null;
		}
	}
}