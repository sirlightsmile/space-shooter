
using UnityEngine;

namespace SmileProject.Generic
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T GetInstance()
		{
			if (_instance == null)
			{
				GameObject instanceObj = new GameObject($"{typeof(T).Name}_Instance");
				_instance = instanceObj.AddComponent<T>();
				DontDestroyOnLoad(instanceObj);
			}
			return _instance;
		}

		protected virtual void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this);
				throw new System.Exception("An instance of this singleton already exists.");
			}
			else
			{
				_instance = this.GetComponent<T>();
			}
		}

		private void OnDestroy()
		{
			_instance = null;
		}

		private void OnApplicationQuit()
		{
			_instance = null;
		}
	}
}