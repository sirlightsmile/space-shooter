using System;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameController : MonoSingleton<GameController>
	{
		public EventHandler GameDataInitialized;
		public GameDataManager GameDataManager { get; private set; }

		void Start()
		{
			AsyncInitialize();
		}

		/// <summary>
		/// Async initialize. Call only once per session.
		/// </summary>
		/// <returns></returns>
		private async void AsyncInitialize()
		{
			await ResourceLoader.InitializeAsync();

			GameDataManager = new GameDataManager();
			await GameDataManager.Initialize();
			GameDataInitialized?.Invoke(this, new EventArgs());
		}
	}
}