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

		private FormationController formationController;
		private List<EnemySpaceship> enemySpaceships = new List<EnemySpaceship>();

		/// <summary>
		/// Shoot chance in percent (max is 1)
		/// </summary>
		private float randomShootChance = 0.3f;

		/// <summary>
		/// Reference of time that trigger spaceships shoot
		/// </summary>
		private float lastShootTimestamp = 0;

		/// <summary>
		/// Time interval for trigger spaceships shoot (seconds)
		/// </summary>
		private float triggerShootInterval = 2f;

		/// <summary>
		/// Max random time for shoot async (seconds)
		/// </summary>
		private float shootAsyncInterval = 1f;

		/// <summary>
		/// Min time interval for random triggerPointBlankInterval (seconds)
		/// </summary>
		private float minPointBlankInterval = 8f;

		/// <summary>
		/// Max time interval for random triggerPointBlankInterval (seconds)
		/// </summary>
		private float maxPointBlankInterval = 15f;

		/// <summary>
		/// Time interval for trigger point-blank shot (seconds)
		/// </summary>
		private float triggerPointBlankInterval = 8f;

		/// <summary>
		/// Reference of time that trigger point-blank shot
		/// </summary>
		private float lastPointBlankTimestamp = 0;

		private GameplayController gameplayController;

		public EnemyManager(GameplayController gameplayController, FormationController formationController)
		{
			this.gameplayController = gameplayController;
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
			if (currentTime - lastShootTimestamp < triggerShootInterval)
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
					TriggerShootAsync(spaceship);
				}
			}
		}

		private void RandomPointBlankAttack(float currentTime)
		{
			if (currentTime - lastPointBlankTimestamp < triggerPointBlankInterval)
			{
				return;
			}
			lastPointBlankTimestamp = currentTime;
			var target = gameplayController.GetPlayerSpaceship();
			if (target != null)
			{
				Vector2 targetPos = target.transform.position;
				TriggerPointBlankAttack(targetPos);
				triggerPointBlankInterval = UnityEngine.Random.Range(minPointBlankInterval, maxPointBlankInterval);
			}
		}


		private EnemySpaceship GetRandomSpaceship()
		{
			int index = UnityEngine.Random.Range(0, enemySpaceships.Count);
			return enemySpaceships[index];
		}

		private void TriggerPointBlankAttack(Vector2 target)
		{
			EnemySpaceship[] availableSpaceships = enemySpaceships.Where(o => !o.IsPerformPointBlank).ToArray();
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
			spaceship.PointBlankAttack(target);
			spaceship.PlaySound(GameSoundKeys.Drone);
		}

		private async void TriggerShootAsync(EnemySpaceship spaceship)
		{
			float randomDelay = UnityEngine.Random.Range(0f, shootAsyncInterval);
			// from second to millisecond
			int delayTimeMillisecond = (int)(randomDelay * 1000);
			await Task.Delay(delayTimeMillisecond);
			if (spaceship != null && spaceship.transform != null)
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