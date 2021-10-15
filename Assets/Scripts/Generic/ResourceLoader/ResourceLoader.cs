using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;

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

		public async Task<T> LoadJsonAsModel<T>(string key)
		{
			TextAsset textAsset = await Load<TextAsset>(key);
			T model = JsonConvert.DeserializeObject<T>(textAsset.text);
			return model;
		}

		public async void SetSpriteAsync(string key, SpriteRenderer spriteRenderer)
		{
			Sprite sprite = await Load<Sprite>(key);
			spriteRenderer.sprite = sprite;
		}
	}
}