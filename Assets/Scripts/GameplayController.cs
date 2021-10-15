using System;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameplayController : MonoBehaviour
	{
		public event EventHandler Pause;

		public event EventHandler Resume;

		public event WaveChangeEventHandler WaveChange;
		public delegate void WaveChangeEventHandler(int newWave);

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

		[SerializeField]
		private PlayerSpaceship playerPrefab;

		private PlayerController playerController;
		private WeaponFactory weaponFactory;
		private GameDataManager gameDataManager;
		private SpaceshipBuilder spaceshipBuilder;

		private int currentWave;

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public void Initialize(GameDataManager gameDataManager, IResourceLoader resourceLoader)
		{
			weaponFactory = new WeaponFactory(gameDataManager);
			playerController = new PlayerController();
			spaceshipBuilder = new SpaceshipBuilder(resourceLoader);

			Timer = 0;
			IsPause = true;
			InitPlayer();
		}

		public void GameStart()
		{
			IsPause = false;
		}

		public void SetGamePause(bool isPause)
		{
			IsPause = isPause;
			if (isPause)
			{
				Pause?.Invoke(this, new EventArgs());
			}
			else
			{
				Resume?.Invoke(this, new EventArgs());
			}
		}

		public void GameOver()
		{
			IsPause = true;
		}

		public void InitPlayer()
		{
			Vector2 spawnPoint = playerSpawnPoint;
			PlayerSpaceship player = Instantiate<PlayerSpaceship>(playerPrefab, spawnPoint, Quaternion.identity);
			SpaceshipGun startGun = weaponFactory.CreateRandomSpaceshipGun();
			player.SetWeapon(startGun);
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