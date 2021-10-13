using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameController : MonoSingleton<GameController>
	{
		public GameDataManager GameDataManager { get; private set; }

		/// <summary>
		/// Async initialize. Call only once per session.
		/// </summary>
		/// <returns></returns>
		public async void AsyncInitialize()
		{
			Debug.Log("Start initializing.");
			await ResourceLoader.InitializeAsync();
			Debug.Log("Addressable initialized");

			Debug.Log("Start initializing game data.");
			GameDataManager = new GameDataManager();
			await GameDataManager.Initialize();
			Debug.Log("Game data initialized.");
		}

		private void Awake()
		{
			AsyncInitialize();
		}
	}
}