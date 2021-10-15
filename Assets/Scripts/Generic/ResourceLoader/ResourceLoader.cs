using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;
using System;

namespace SmileProject.Generic
{
	/// <summary>
	/// Addressable resource loader manager
	/// </summary>
	public class ResourceLoader : IResourceLoader
	{
		public async Task InitializeAsync()
		{
			AsyncOperationHandle<IResourceLocator> initialize = Addressables.InitializeAsync();
			await initialize.Task;
			Debug.Log("ResourceLoader initialized");
		}

		public async Task<T> Load<T>(string key)
		{
			AsyncOperationHandle<T> loadAssetAsync = Addressables.LoadAssetAsync<T>(key);
			loadAssetAsync.Completed += (operation) =>
			{
				if (operation.Status == AsyncOperationStatus.Failed)
				{
					Debug.LogError($"Failed to load asset : [key]");
				}
			};
			T loadedAsset = await loadAssetAsync.Task;
			return loadedAsset;
		}

		public async Task<T> InstantiateAsync<T>(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true) where T : MonoBehaviour
		{
			AsyncOperationHandle<GameObject> loadAssetAsync = Addressables.InstantiateAsync(key, parent);
			loadAssetAsync.Completed += (operation) =>
			{
				if (operation.Status == AsyncOperationStatus.Failed)
				{
					Debug.LogError($"Failed to instantiate asset : [key]");
				}
			};
			GameObject instantiatedObject = await loadAssetAsync.Task;
			T component = instantiatedObject.GetComponent<T>();
			if (component == null)
			{
				Debug.LogError($"Failed to get component [{typeof(T).Name}] from instantiate object");
			}
			return component;
		}

		public async Task<T> LoadJsonAsModel<T>(string key)
		{
			TextAsset textAsset = await Load<TextAsset>(key);
			T model = JsonConvert.DeserializeObject<T>(textAsset.text);
			return model;
		}

		public async void SetSpriteAsync(string key, SetSpriteHandler spriteHandler)
		{
			Sprite sprite = await Load<Sprite>(key);
			spriteHandler?.Invoke(sprite);
		}
	}
}