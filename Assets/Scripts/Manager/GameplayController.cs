using System;
using System.Threading.Tasks;
using SmileProject.Generic;
using SmileProject.SpaceShooter.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		private bool isGameEnd = false;

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

			// setup listener
			inputManager.ConfirmInput += ResetGame;
			inputManager.MenuInput += () => { SetGamePause(!IsPause); };
			enemyManager.AllSpaceshipDestroyed += OnWaveClear;
			playerController.PlayerDestroyed += OnPlayerDestroyed;
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
			isGameEnd = false;
			PlayGameplayBGM();
			Start?.Invoke();
			NextWave();
		}

		private void ResetGame()
		{
			if (!isGameEnd)
			{
				return;
			}
			Debug.Log("Reset scene");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private void SetGamePause(bool isPause)
		{
			IsPause = isPause;
			Pause?.Invoke(isPause);
			uiManager.SetGameplayMenu(isPause);
			Time.timeScale = isPause ? 0f : 1f;
		}

		private async void ClearGame()
		{
			GameEnd();
			uiManager.ShowGameClear(playerScore);
			await audioManager.PlaySound(GameSoundKeys.Succeed);
		}

		private async void GameOver()
		{
			GameEnd();
			uiManager.ShowGameOver();
			await audioManager.PlaySound(GameSoundKeys.Failed);
		}

		private void GameEnd()
		{
			isGameEnd = true;
			IsPause = true;
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
			if (!isReady)
			{
				return;
			}
			inputManager.SetAllowAttack(true);
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

		private void OnPlayerDestroyed()
		{
			GameOver();
		}

		private void Update()
		{
			if (IsPause)
			{
				return;
			}

			enemyManager.Update();
			Timer += Time.time;
		}
	}
}