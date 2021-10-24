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
		private EnemyManager enemyManager;
		private InputManager inputManager;
		private AudioManager audioManager;

		private int currentWave = firstWave;
		private int waveCount;

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public async Task Initialize(PlayerController playerController, EnemyManager enemyManager, InputManager inputManager, AudioManager audioManager)
		{
			this.playerController = playerController;
			this.inputManager = inputManager;
			this.audioManager = audioManager;
			this.enemyManager = enemyManager;
			enemyManager.AllSpaceshipDestroyed += OnWaveClear;
			playerController.PlayerDestroyed += OnGameOver;

			IsPause = true;
			await playerController.CreatePlayer(playerSpawnPoint);
		}

		/// <summary>
		/// Set total wave that enemies will spawn
		/// </summary>
		/// <param name="wave">total wave</param>
		public void SetWaveCount(int wave)
		{
			this.waveCount = wave;
		}

		public void GameStart()
		{
			Timer = 0;
			IsPause = false;
			PlayGameplayBGM();
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

		public void ClearGame()
		{
			Debug.Log("Clear game !");
		}

		private async void PlayGameplayBGM()
		{
			await audioManager.PlaySound(GameSoundKeys.GameplayBGM, true);
		}

		private async void NextWave()
		{
			await Task.Delay(waveInterval);
			currentWave++;
			WaveChange?.Invoke(currentWave);
		}

		private void OnWaveClear()
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

		private void OnGameOver()
		{
			IsPause = true;
		}

		private void Update()
		{
			if (IsPause)
			{
				return;
			}

			inputManager.Update();
			enemyManager.Update();
			Timer += Time.time;
		}
	}
}