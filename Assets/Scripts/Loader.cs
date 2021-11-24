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

		private GameDataManager _gameDataManager;
		private AddressableResourceLoader _resourceLoader;
		private GameplayController _gameplayController;
		private PoolManager _poolManager;
		private AudioManager _audioManager;

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
			_resourceLoader = new AddressableResourceLoader();
			_gameDataManager = new GameDataManager();

			await _resourceLoader.InitializeAsync();
			await Task.WhenAll(new Task[]
			{
				_gameDataManager.Initialize(_resourceLoader),
				InitPoolManager(_resourceLoader),
				InitAudioManager(_resourceLoader)
			});
			await InitGameplayController(_resourceLoader, _gameDataManager, _poolManager, _audioManager);

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

			_gameplayController = gameplayController;
			_gameplayController.StandBy();
		}

		private async Task InitPoolManager(IResourceLoader resourceLoader)
		{
			PoolManager poolManager = await resourceLoader.InstantiateAsync<PoolManager>("PoolManager");
			_poolManager = poolManager;
			_poolManager.Initialize(resourceLoader);
		}

		private async Task InitAudioManager(IResourceLoader resourceLoader)
		{
			AudioManager audioManager = await resourceLoader.InstantiateAsync<AudioManager>("AudioManager");
			_audioManager = audioManager;
			await _audioManager.Initialize(resourceLoader, MixerGroup.MAIN_MIXER_KEY);
		}
	}
}