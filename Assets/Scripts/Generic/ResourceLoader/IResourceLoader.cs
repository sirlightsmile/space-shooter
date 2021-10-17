using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic
{
	public delegate void SetSpriteHandler(Sprite sprite);
	public interface IResourceLoader
	{
		Task InitializeAsync();
		Task<T> Load<T>(string key);
		Task<T> LoadPrefab<T>(string key);
		Task<T> LoadJsonAsModel<T>(string key);
		Task<T> InstantiateAsync<T>(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true) where T : MonoBehaviour;
		void SetSpriteAsync(string key, SetSpriteHandler eventHandler);
	}
}