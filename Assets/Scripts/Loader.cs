using System;
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class Loader : MonoSingleton<Loader>
	{
		public event EventHandler Initialized;

		public bool IsInitialized { get; private set; }

		private GameDataManager gameDataManager;
		private ResourceLoader resourceLoader;
		private GameplayController gameplayController;
		private PoolManager poolManager;

		private void Start()
		{
			Initialize();
		}

		/// <summary>
		/// Async initialize. Call only once per session.
		/// </summary>
		/// <returns></returns>
		private async void Initialize()
		{
			resourceLoader = new ResourceLoader();
			gameDataManager = new GameDataManager();

			await resourceLoader.InitializeAsync();
			await Task.WhenAll(new Task[]
			{
				gameDataManager.Initialize(resourceLoader),
				InitPoolManager(resourceLoader)
			});
			await InitGameplayController(resourceLoader, gameDataManager, poolManager);

			IsInitialized = true;
			Initialized?.Invoke(this, new EventArgs());
		}

		private async Task InitGameplayController(ResourceLoader resourceLoader, GameDataManager gameDataManager, PoolManager poolManager)
		{
			GameplayController gameplayController = await resourceLoader.InstantiateAsync<GameplayController>("GameplayController");
			this.gameplayController = gameplayController;
			await this.gameplayController.Initialize(gameDataManager, resourceLoader, poolManager);
		}

		private async Task InitPoolManager(ResourceLoader resourceLoader)
		{
			PoolManager poolManager = await resourceLoader.InstantiateAsync<PoolManager>("PoolManager");
			this.poolManager = poolManager;
			this.poolManager.Initialize(resourceLoader);
		}
	}
}