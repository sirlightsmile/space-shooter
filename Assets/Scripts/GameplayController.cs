using System;
using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameplayController : MonoBehaviour
	{
		private const int firstWave = 1;
		private const int waveInterval = 5000;

		public event Action Start;
		public event Action<bool> Pause;

		/// <summary>
		/// Event invoke when wave changed
		/// </summary>
		public event Action<int> WaveChange;

		/// <summary>
		/// Is Game pause
		/// </summary>
		/// <value></value>
		public bool IsPause { get; private set; } = true;

		/// <summary>
		/// Game timer. Start counting on GameStart and stop when pause or game over.
		/// </summary>
		/// <value></value>
		public float Timer { get; private set; }

		[SerializeField]
		private Vector2 playerSpawnPoint;

		private PlayerController playerController;
		private WeaponFactory weaponFactory;
		private EnemyManager enemyManager;
		private GameDataManager gameDataManager;
		private IResourceLoader resourceLoader;
		private InputManager inputManager;
		private AudioManager audioManager;

		private int currentWave = firstWave;
		private int waveCount;

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public async Task Initialize(GameDataManager gameDataManager, IResourceLoader resourceLoader, PoolManager poolManager, AudioManager audioManager)
		{
			this.resourceLoader = resourceLoader;
			this.gameDataManager = gameDataManager;
			this.inputManager = new InputManager();
			playerController = new PlayerController(inputManager);
			weaponFactory = new WeaponFactory(gameDataManager, poolManager, audioManager);
			FormationController enemyFormationController = await resourceLoader.InstantiateAsync<FormationController>("FormationController");
			enemyManager = new EnemyManager(this, resourceLoader, gameDataManager, enemyFormationController);
			enemyManager.AllSpaceshipDestroyed += WaveClear;

			int waveCount = gameDataManager.GetWaveDataModels().Length;
			Timer = 0;
			IsPause = true;
			await InitPlayer();
			GameStart();
		}

		private void WaveClear()
		{
			if (waveCount > currentWave)
			{
				NextWave();
			}
			else
			{
				ClearGame();
			}
		}

		private async void NextWave()
		{
			await Task.Delay(waveInterval);
			currentWave++;
			WaveChange?.Invoke(currentWave);
		}

		private void GameStart()
		{
			IsPause = false;
			PlayGameplayBGM();
			Start?.Invoke();
			WaveChange?.Invoke(currentWave);
		}

		private async void PlayGameplayBGM()
		{
			await audioManager.PlaySound(GameSoundKeys.GameplayBGM);
		}

		public void SetGamePause(bool isPause)
		{
			IsPause = isPause;
			if (isPause)
			{
				Pause?.Invoke(isPause);
			}
		}

		public void GameOver()
		{
			IsPause = true;
		}

		public void ClearGame()
		{
			Debug.Log("Clear game !");
		}

		public async Task InitPlayer()
		{
			PlayerSpaceshipBuilder builder = playerController.CreatePlayerBuilder(resourceLoader, weaponFactory, gameDataManager);
			await playerController.CreatePlayer(playerSpawnPoint, builder);
			playerController.PlayerDestroyed += GameOver;
		}

		private void Update()
		{
			if (IsPause)
			{
				return;
			}

			inputManager.Update();
			Timer += Time.time;
		}
	}
}