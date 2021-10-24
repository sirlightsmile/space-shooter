using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemyManager
	{
		public event Action AllSpaceshipDestroyed;
		public bool IsEnemiesReady { get; private set; }
		private FormationController formationController;
		private List<EnemySpaceship> enemySpaceships = new List<EnemySpaceship>();

		private float randomShootChance = 0.3f;
		private float shootInterval = 2f;
		private float lastShootTimestamp = 0;


		public EnemyManager(GameplayController gameplayController, FormationController formationController)
		{
			this.formationController = formationController;
			formationController.SpaceshipAdded += OnEnemySpaceshipAdded;
			formationController.FormationChange += OnFormationChanged;
			formationController.FormationReady += OnFormationReady;
			gameplayController.WaveChange += formationController.OnWaveChanged;

		}

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
				float random = UnityEngine.Random.Range(0f, 1f);
				bool isShoot = random <= randomShootChance;
				if (isShoot)
				{
					spaceship.Shoot();
				}
			}
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
			enemySpaceships.Remove(spaceship.GetComponent<EnemySpaceship>());
			if (enemySpaceships.Count == 0)
			{
				AllSpaceshipDestroyed?.Invoke();
			}
		}
	}
}