using System;
using System.Collections.Generic;
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
		/// Whather enemy ready to fight or not
		/// </summary>
		/// <value></value>
		public bool IsEnemiesReady { get; private set; }

		private FormationController formationController;
		private List<EnemySpaceship> enemySpaceships = new List<EnemySpaceship>();

		/// <summary>
		/// Shoot chance in percent (max is 1)
		/// </summary>
		private float randomShootChance = 0.3f;
		private float lastShootTimestamp = 0;

		/// <summary>
		/// Shoot interval in second
		/// </summary>
		private float shootInterval = 2f;

		/// <summary>
		/// Max random shoot async in second
		/// </summary>
		private float shootAsyncInterval = 1f;

		public EnemyManager(GameplayController gameplayController, FormationController formationController)
		{
			this.formationController = formationController;
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
			float currentTime = Time.time;
			if (currentTime - lastShootTimestamp < shootInterval)
			{
				return;
			}

			lastShootTimestamp = currentTime;
			foreach (var spaceship in enemySpaceships)
			{
				// random for percent
				float random = UnityEngine.Random.Range(0f, 1f);
				bool isShoot = random <= randomShootChance;
				if (isShoot)
				{
					ShootAsync(spaceship);
				}
			}
		}

		private async void ShootAsync(EnemySpaceship spaceship)
		{
			float randomDelay = UnityEngine.Random.Range(0f, shootAsyncInterval);
			// from second to millisecond
			int delayTimeMillisecond = (int)(randomDelay * 1000);
			await Task.Delay(delayTimeMillisecond);
			spaceship?.Shoot();
		}

		private void OnFormationChanged()
		{
			IsEnemiesReady = false;
		}

		private void OnFormationReady()
		{
			IsEnemiesReady = true;
		}

		private void OnEnemySpaceshipAdded(Spaceship spaceship)
		{
			spaceship.Destroyed += OnEnemyDestroyed;
			enemySpaceships.Add(spaceship.GetComponent<EnemySpaceship>());
		}

		private void OnEnemyDestroyed(Spaceship spaceship)
		{
			spaceship.Destroyed -= OnEnemyDestroyed;
			EnemySpaceship enemy = spaceship.GetComponent<EnemySpaceship>();
			enemySpaceships.Remove(enemy);
			EnemyDestroyed?.Invoke(enemy.DestroyScore);
			if (enemySpaceships.Count == 0)
			{
				AllSpaceshipDestroyed?.Invoke();
			}
		}
	}
}