using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	public static class ResourceLoader
	{
		private const string gameDataAddress = "GameData";

		public static async Task InitializeAsync()
		{
			AsyncOperationHandle<IResourceLocator> initialize = Addressables.InitializeAsync();
			await initialize.Task;
		}


		public static async Task<GameDataModel> LoadGameData()
		{
			var loadGameDataAsync = Addressables.LoadAssetAsync<TextAsset>(gameDataAddress);
			TextAsset gameDataStr = await loadGameDataAsync.Task;
			GameDataModel gameDataModel = JsonConvert.DeserializeObject<GameDataModel>(gameDataStr.text);
			return gameDataModel;
		}
	}
}