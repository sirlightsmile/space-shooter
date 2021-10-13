using System;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameplayController : MonoSingleton<GameController>
	{
		public EventHandler Pause;

		public EventHandler Resume;

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

		void Awake()
		{
			GameController.GetInstance().GameDataInitialized += OnGameDataInitialized;
		}

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public void Initialize()
		{
			Timer = 0;
			IsPause = true;
			playerController = new PlayerController();
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

			//TODO: set player gun from game data later

			// WeaponFactory factory = new WeaponFactory(Game);

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

		private void OnGameDataInitialized(System.Object sender, EventArgs args)
		{
			Initialize();
			GameStart();
		}
	}
}