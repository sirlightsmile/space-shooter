using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemyManager
	{
		/// <summary>
		/// Invoke when enemy destroyed with enemy destroy score
		/// </summary>
		public event Action<int> EnemyDestroyed;

		/// <summary>
		/// Invoke when all enemies spaceship in wave had destroyed
		/// </summary>
		public event Action AllSpaceshipDestroyed;

		/// <summary>
		/// Invoke when is enemy ready status changed
		/// </summary>
		public event Action<bool> EnemyReadyStatusChanged;

		/// <summary>
		/// Whather enemy ready to fight or not
		/// </summary>
		/// <value></value>
		public bool IsEnemiesReady { get; private set; }

		private FormationController _formationController;
		private GameplayController _gameplayController;
		private List<EnemySpaceship> _enemySpaceships = new List<EnemySpaceship>();

		/// <summary>
		/// Shoot chance in percent (max is 1)
		/// </summary>
		private float _randomShootChance = 0.3f;

		/// <summary>
		/// Reference of time that trigger spaceships shoot
		/// </summary>
		private float _lastShootTimestamp = 0;

		/// <summary>
		/// Time interval for trigger spaceships shoot (seconds)
		/// </summary>
		private float _triggerShootInterval = 2f;

		/// <summary>
		/// Max random time for shoot async (seconds)
		/// </summary>
		private float _shootAsyncInterval = 1f;

		/// <summary>
		/// Min time interval for random triggerPointBlankInterval (seconds)
		/// </summary>
		private float _minPointBlankInterval = 8f;

		/// <summary>
		/// Max time interval for random triggerPointBlankInterval (seconds)
		/// </summary>
		private float _maxPointBlankInterval = 15f;

		/// <summary>
		/// Time interval for trigger point-blank shot (seconds)
		/// </summary>
		private float _triggerPointBlankInterval = 8f;

		/// <summary>
		/// Reference of time that trigger point-blank shot
		/// </summary>
		private float _lastPointBlankTimestamp = 0;

		public EnemyManager(GameplayController gameplayController, FormationController formationController)
		{
			_gameplayController = gameplayController;
			_formationController = formationController;
			formationController.SpaceshipAdded += OnEnemySpaceshipAdded;
			formationController.FormationChange += OnFormationChanged;
			formationController.FormationReady += OnFormationReady;
			gameplayController.WaveChange += formationController.OnWaveChanged;
		}

		/// <summary>
		/// Enemy manager update loop
		/// Should manual update by gameplay controller
		/// </summary>
		public void Update()
		{
			if (!IsEnemiesReady)
			{
				return;
			}

			float currentTime = Time.time;
			RandomEnemiesShoot(currentTime);
			RandomPointBlankAttack(currentTime);
		}

		private void RandomEnemiesShoot(float currentTime)
		{
			if (currentTime - _lastShootTimestamp < _triggerShootInterval)
			{
				return;
			}

			_lastShootTimestamp = currentTime;
			foreach (var spaceship in _enemySpaceships)
			{
				if (spaceship.IsPerformPointBlank)
				{
					continue;
				}

				// random for shoot percent
				float random = UnityEngine.Random.Range(0f, 1f);
				bool isShoot = random <= _randomShootChance;
				if (isShoot)
				{
					var _ = TriggerShootAsync(spaceship);
				}
			}
		}

		private void RandomPointBlankAttack(float currentTime)
		{
			if (currentTime - _lastPointBlankTimestamp < _triggerPointBlankInterval)
			{
				return;
			}
			_lastPointBlankTimestamp = currentTime;
			var target = _gameplayController.GetPlayerSpaceship();
			if (target != null)
			{
				TriggerPointBlankAttack(target.transform);
				_triggerPointBlankInterval = UnityEngine.Random.Range(_minPointBlankInterval, _maxPointBlankInterval);
			}
		}


		private EnemySpaceship GetRandomSpaceship()
		{
			int index = UnityEngine.Random.Range(0, _enemySpaceships.Count);
			return _enemySpaceships[index];
		}

		private void TriggerPointBlankAttack(Transform target)
		{
			EnemySpaceship[] availableSpaceships = _enemySpaceships.Where(o => !o.IsPerformPointBlank).ToArray();
			EnemySpaceship spaceship = null;
			int shipCount = availableSpaceships.Length;
			if (shipCount > 0)
			{
				int index = UnityEngine.Random.Range(0, shipCount);
				spaceship = availableSpaceships[index];
			}
			if (spaceship == null)
			{
				Debug.Log("No spaceship available for point-blank shot");
				return;
			}
			var _ = spaceship.PointBlankAttack(target);
		}

		private async Task TriggerShootAsync(EnemySpaceship spaceship)
		{
			float randomDelay = UnityEngine.Random.Range(0f, _shootAsyncInterval);
			// from second to millisecond
			int delayTimeMillisecond = (int)(randomDelay * 1000);
			await Task.Delay(delayTimeMillisecond);
			if (spaceship != null && spaceship.IsActive)
			{
				spaceship?.Shoot();
			}
		}

		private void OnFormationChanged()
		{
			IsEnemiesReady = false;
			EnemyReadyStatusChanged?.Invoke(IsEnemiesReady);
		}

		private void OnFormationReady()
		{
			IsEnemiesReady = true;
			EnemyReadyStatusChanged?.Invoke(IsEnemiesReady);
		}

		private void OnEnemySpaceshipAdded(Spaceship spaceship)
		{
			spaceship.Destroyed += OnEnemyDestroyed;
			_enemySpaceships.Add(spaceship.GetComponent<EnemySpaceship>());
		}

		private void OnEnemyDestroyed(Spaceship spaceship)
		{
			spaceship.Destroyed -= OnEnemyDestroyed;
			EnemySpaceship enemy = spaceship.GetComponent<EnemySpaceship>();
			_enemySpaceships.Remove(enemy);
			EnemyDestroyed?.Invoke(enemy.DestroyScore);
			if (_enemySpaceships.Count == 0)
			{
				AllSpaceshipDestroyed?.Invoke();
			}
		}
	}
}