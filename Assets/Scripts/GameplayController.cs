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
		private GameplayUIManager uiManager;

		private int currentWave = firstWave;
		private int waveCount;
		private int playerScore = 0;

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public async Task Initialize(PlayerController playerController, EnemyManager enemyManager, InputManager inputManager, AudioManager audioManager, GameplayUIManager uiManager)
		{
			this.playerController = playerController;
			this.inputManager = inputManager;
			this.audioManager = audioManager;
			this.enemyManager = enemyManager;
			this.uiManager = uiManager;


			enemyManager.AllSpaceshipDestroyed += OnWaveClear;
			playerController.PlayerDestroyed += OnGameOver;

			// ui
			playerController.PlayerGetHit += OnPlayerGetHit;
			enemyManager.EnemyDestroyed += OnEnemyDestroyed;

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
			playerScore = 0;
			Timer = 0;
			IsPause = false;
			PlayGameplayBGM();
			Start?.Invoke();
			WaveChange?.Invoke(currentWave);
		}

		public void SetGamePause(bool isPause)
		{
			IsPause = isPause;
			Pause?.Invoke(isPause);
			Time.timeScale = isPause ? 0f : 1f;
		}

		public void ClearGame()
		{
			Debug.Log("Clear game !");
		}

		private async void PlayGameplayBGM()
		{
			await audioManager.PlaySound(GameSoundKeys.GameplayBGM, true);
		}

		private void OnEnemyDestroyed(int score)
		{
			playerController.AddScore(score);
			uiManager.SetPlayerScore(playerController.PlayerScore);
		}

		private void OnPlayerGetHit(int hp)
		{
			uiManager.SetPlayerHp(hp);
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