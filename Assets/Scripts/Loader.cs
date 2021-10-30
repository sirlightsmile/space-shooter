using System;
using System.Threading.Tasks;
using SmileProject.Generic;
using SmileProject.SpaceShooter.UI;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Loader : MonoBehaviour
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
		public async void Initialize()
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
		public async Task InitGameplayController(IResourceLoader resourceLoader, GameDataManager gameDataManager, PoolManager poolManager, AudioManager audioManager)
		{
			// init async
			FormationController enemyFormationController = null;
			GameplayController gameplayController = null;
			GameplayUIManager uiManager = null;
			InputManager inputManager = null;
			Func<Task> initInputManager = async () => { inputManager = await resourceLoader.InstantiateAsync<InputManager>("InputManager"); };
			Func<Task> initFormationController = async () => { enemyFormationController = await resourceLoader.InstantiateAsync<FormationController>("FormationController"); };
			Func<Task> initGameplayController = async () => { gameplayController = await resourceLoader.InstantiateAsync<GameplayController>("GameplayController"); };
			Func<Task> initGameplayUIManager = async () => { uiManager = await resourceLoader.InstantiateAsync<GameplayUIManager>("GameplayUIManager"); };
			await Task.WhenAll(new Task[] { initInputManager(), initFormationController(), initGameplayController(), initGameplayUIManager() });

			// inject player controller
			WeaponFactory weaponFactory = new WeaponFactory(gameDataManager, poolManager, audioManager);
			PlayerSpaceshipBuilder playerBuilder = new PlayerSpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory, audioManager);
			PlayerController playerController = new PlayerController(inputManager, playerBuilder);

			// inject enemy manager
			EnemySpaceshipBuilder enemiesBuilder = new EnemySpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory, audioManager);

			enemyFormationController.Initialize(gameDataManager, enemiesBuilder);
			EnemyManager enemyManager = new EnemyManager(gameplayController, enemyFormationController);


			int waveCount = gameDataManager.GetWaveDataModels().Length;
			gameplayController.SetWaveCount(waveCount);

			await Task.WhenAll
			(
				new Task[]
				{
					gameplayController.Initialize(playerController, enemyManager, inputManager, audioManager, uiManager),
					enemiesBuilder.SetupSpaceshipPool(poolManager)
				}
			);

			this.gameplayController = gameplayController;
			this.gameplayController.GameStart();
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