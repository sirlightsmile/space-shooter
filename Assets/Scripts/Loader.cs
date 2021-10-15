using System;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class Loader : MonoSingleton<Loader>
	{
		public event EventHandler Initialized;

		private GameDataManager gameDataManager;
		private ResourceLoader resourceLoader;

		private void Start()
		{
			AsyncInitialize();
		}

		/// <summary>
		/// Async initialize. Call only once per session.
		/// </summary>
		/// <returns></returns>
		private async void AsyncInitialize()
		{
			resourceLoader = new ResourceLoader();
			gameDataManager = new GameDataManager();

			await resourceLoader.InitializeAsync();
			await gameDataManager.Initialize(resourceLoader);
			Initialized?.Invoke(this, new EventArgs());
		}
	}
}