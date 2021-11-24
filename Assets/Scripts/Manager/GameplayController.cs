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

		/// <summary>
		/// Constant time (ms) before each wave start
		/// </summary>
		private const int _WAVE_INTERVAL = 3000;

		[SerializeField]
		private Vector2 _playerSpawnPoint;

		private PlayerController _playerController;
		private EnemyManager _enemyManager;
		private InputManager _inputManager;
		private AudioManager _audioManager;
		private GameplayUIManager _uiManager;

		private int _currentWave, _waveCount = 0;
		private int _playerScore = 0;
		private bool _isGameEnded, _isGameStarted = false;

		/// <summary>
		/// Initialize gameplay controller
		/// </summary>
		public async Task Initialize(PlayerController playerController, EnemyManager enemyManager, InputManager inputManager, AudioManager audioManager, GameplayUIManager uiManager)
		{
			_playerController = playerController;
			_inputManager = inputManager;
			_audioManager = audioManager;
			_enemyManager = enemyManager;
			_uiManager = uiManager;

			// setup listener
			inputManager.ConfirmInput += OnPressConfirm;
			inputManager.MenuInput += () => { SetGamePause(!IsPause); };
			enemyManager.AllSpaceshipDestroyed += OnWaveClear;
			playerController.PlayerDestroyed += OnPlayerDestroyed;
			playerController.PlayerGetHit += OnPlayerGetHit;
			enemyManager.EnemyDestroyed += OnEnemyDestroyed;
			enemyManager.EnemyReadyStatusChanged += OnEnemyReadyStatusChanged;

			IsPause = true;
			await playerController.CreatePlayer(_playerSpawnPoint);
			uiManager.SetPlayerHp(playerController.PlayerSpaceship.HP);
		}

		/// <summary>
		/// Get current player's spaceship
		/// </summary>
		/// <returns></returns>
		public PlayerSpaceship GetPlayerSpaceship()
		{
			return _playerController.PlayerSpaceship;
		}

		/// <summary>
		/// Set total wave that enemies will spawn
		/// </summary>
		/// <param name="wave">total wave</param>
		public void SetWaveCount(int wave)
		{
			_waveCount = wave;
		}

		public void StandBy()
		{
			_uiManager.SetShowGameStart(true);
		}

		private void GameStart()
		{
			Timer = 0;
			IsPause = false;
			_playerScore = 0;
			_currentWave = 0;
			_isGameEnded = false;
			_isGameStarted = true;
			_uiManager.SetShowGameStart(false);
			Start?.Invoke();
			PlayGameplayBGM();
			var _ = NextWave();
		}

		private void OnPressConfirm()
		{
			if (!_isGameStarted)
			{
				GameStart();
			}
			else if (_isGameEnded)
			{
				ResetGame();
			}
		}

		private void ResetGame()
		{
			Debug.Log("Reset scene");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private void SetGamePause(bool isPause)
		{
			IsPause = isPause;
			Pause?.Invoke(isPause);
			_uiManager.SetGameplayMenu(isPause);
			Time.timeScale = isPause ? 0f : 1f;
		}

		private void ClearGame()
		{
			GameEnd();
			_uiManager.ShowGameClear(_playerScore);
			var _ = _audioManager.PlaySound(GameSoundKeys.Succeed);
		}

		private void GameOver()
		{
			GameEnd();
			_uiManager.ShowGameOver();
			var _ = _audioManager.PlaySound(GameSoundKeys.Failed);
		}

		private void GameEnd()
		{
			_isGameEnded = true;
			IsPause = true;
		}

		private void PlayGameplayBGM()
		{
			var _ = _audioManager.PlaySound(GameSoundKeys.GameplayBGM, true);
		}

		private void OnEnemyDestroyed(int score)
		{
			_playerController.AddScore(score);
			_uiManager.SetPlayerScore(_playerController.PlayerScore);
		}

		private void OnEnemyReadyStatusChanged(bool isReady)
		{
			if (!isReady)
			{
				return;
			}
			_inputManager.SetAllowAttack(true);
		}

		private void OnPlayerGetHit(int hp)
		{
			_uiManager.SetPlayerHp(hp);
		}

		private async Task NextWave()
		{
			_uiManager.ShowWaveChange(_currentWave + 1, _WAVE_INTERVAL);
			await Task.Delay(_WAVE_INTERVAL);
			_currentWave++;
			WaveChange?.Invoke(_currentWave);
		}

		private void OnWaveClear()
		{
			// wait for next wave generate
			_inputManager.SetAllowAttack(false);
			if (_waveCount > _currentWave)
			{
				var _ = NextWave();
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
			if (IsPause || _isGameEnded)
			{
				return;
			}

			_enemyManager.Update();
			Timer += Time.time;
		}
	}
}