using System;
using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameplayController : MonoBehaviour
	{
		private const int firstWave = 1;

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

		private int currentWave = firstWave;

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public async Task Initialize(GameDataManager gameDataManager, IResourceLoader resourceLoader, PoolManager poolManager)
		{
			this.resourceLoader = resourceLoader;
			this.gameDataManager = gameDataManager;
			weaponFactory = new WeaponFactory(gameDataManager, poolManager);
			playerController = new PlayerController();
			FormationController enemyFormationController = await resourceLoader.InstantiateAsync<FormationController>("FormationController");
			enemyManager = new EnemyManager(this, resourceLoader, gameDataManager, enemyFormationController);
			enemyManager.AllSpaceshipDestroyed += NextWave;

			Timer = 0;
			IsPause = true;
			await InitPlayer();
			GameStart();
		}

		private void NextWave()
		{
			currentWave++;
			WaveChange?.Invoke(currentWave);
		}

		private void GameStart()
		{
			IsPause = false;
			Start?.Invoke();
			WaveChange?.Invoke(currentWave);
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

		public async Task InitPlayer()
		{
			Vector2 spawnPoint = playerSpawnPoint;
			PlayerSpaceshipBuilder builder = new PlayerSpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory);
			PlayerSpaceship player = await builder.BuildRandomSpaceship();
			player.SetPosition(spawnPoint);
			playerController.SetPlayer(player);
		}

		private void Update()
		{
			if (IsPause)
			{
				return;
			}

			playerController.Update();
			Timer += Time.time;
		}
	}
}