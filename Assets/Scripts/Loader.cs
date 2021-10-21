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
		private AddressableResourceLoader resourceLoader;
		private GameplayController gameplayController;
		private PoolManager poolManager;
		private AudioManager audioManager;

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
			resourceLoader = new AddressableResourceLoader();
			gameDataManager = new GameDataManager();

			await resourceLoader.InitializeAsync();
			await Task.WhenAll(new Task[]
			{
				gameDataManager.Initialize(resourceLoader),
				InitPoolManager(resourceLoader),
				InitAudioManager(resourceLoader)
			});
			await InitGameplayController(resourceLoader, gameDataManager, poolManager, audioManager);

			IsInitialized = true;
			Initialized?.Invoke(this, new EventArgs());
		}

		private async Task InitGameplayController(IResourceLoader resourceLoader, GameDataManager gameDataManager, PoolManager poolManager, AudioManager audioManager)
		{
			GameplayController gameplayController = await resourceLoader.InstantiateAsync<GameplayController>("GameplayController");
			this.gameplayController = gameplayController;
			await this.gameplayController.Initialize(gameDataManager, resourceLoader, poolManager, audioManager);
		}

		private async Task InitPoolManager(IResourceLoader resourceLoader)
		{
			PoolManager poolManager = await resourceLoader.InstantiateAsync<PoolManager>("PoolManager");
			this.poolManager = poolManager;
			this.poolManager.Initialize(resourceLoader);
		}

		private async Task InitAudioManager(IResourceLoader resourceLoader)
		{
			AudioManager audioManager = await resourceLoader.InstantiateAsync<AudioManager>("AudioManager");
			this.audioManager = audioManager;
			await this.audioManager.Initialize(resourceLoader, MixerGroup.MainMixerKey);
		}
	}
}