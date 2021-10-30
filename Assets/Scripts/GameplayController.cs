using System;
using System.Threading.Tasks;
using SmileProject.Generic;
using SmileProject.SpaceShooter.UI;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class GameplayController : MonoBehaviour
	{
		/// <summary>
		/// Invoke when game started
		/// </summary>
		public event Action Start;

		/// <summary>
		/// Invoke when game pause or resume
		/// </summary>
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

		private const int waveInterval = 3000;

		[SerializeField]
		private Vector2 playerSpawnPoint;

		[SerializeField]
		private bool triggerPointBlank;

		private PlayerController playerController;
		private EnemyManager enemyManager;
		private InputManager inputManager;
		private AudioManager audioManager;
		private GameplayUIManager uiManager;

		private int currentWave = 0;
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
			playerController.PlayerGetHit += OnPlayerGetHit;
			enemyManager.EnemyDestroyed += OnEnemyDestroyed;
			enemyManager.EnemyReadyStatusChanged += OnEnemyReadyStatusChanged;

			IsPause = true;
			await playerController.CreatePlayer(playerSpawnPoint);
			uiManager.SetPlayerHp(playerController.PlayerSpaceship.HP);
		}

		/// <summary>
		/// Get current player's spaceship
		/// </summary>
		/// <returns></returns>
		public PlayerSpaceship GetPlayerSpaceship()
		{
			return playerController.PlayerSpaceship;
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
			currentWave = 0;
			IsPause = false;
			PlayGameplayBGM();
			Start?.Invoke();
			NextWave();
		}

		public void SetGamePause(bool isPause)
		{
			IsPause = isPause;
			Pause?.Invoke(isPause);
			Time.timeScale = isPause ? 0f : 1f;
		}

		public void ClearGame()
		{
			//TODO: show clear game with retry UI
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

		private void OnEnemyReadyStatusChanged(bool isReady)
		{
			if (isReady)
			{
				inputManager.SetAllowAttack(true);
			}
			else
			{
				//TODO: show prepare UI
			}
		}

		private void OnPlayerGetHit(int hp)
		{
			uiManager.SetPlayerHp(hp);
		}

		private async void NextWave()
		{
			uiManager.ShowWaveChange(currentWave + 1, waveInterval);
			await Task.Delay(waveInterval);
			currentWave++;
			WaveChange?.Invoke(currentWave);
		}

		private void OnWaveClear()
		{
			// wait for next wave generate
			inputManager.SetAllowAttack(false);
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
			//TODO: show retry UI
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