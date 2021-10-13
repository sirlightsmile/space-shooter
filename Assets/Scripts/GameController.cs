using System;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameController : MonoSingleton<GameController>
	{
		public EventHandler GameDataInitialized;
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

			GameDataInitialized?.Invoke(this, new EventArgs());
		}

		private void Awake()
		{
			AsyncInitialize();
		}
	}
}