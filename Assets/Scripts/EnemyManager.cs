using System;
using System.Collections.Generic;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemyManager
	{
		public event Action AllSpaceshipDestroyed;
		private FormationController formationController;
		private List<Spaceship> enemySpaceships = new List<Spaceship>();

		public EnemyManager(GameplayController gameplayController, IResourceLoader resourceLoader, GameDataManager gameDataManager, FormationController formationController)
		{
			SpaceshipBuilder enemySpaceshipBuilder = new EnemySpaceshipBuilder(resourceLoader, gameDataManager);
			enemySpaceshipBuilder.SpaceshipBuilded += OnEnemyAdded;
			this.formationController = formationController;
			formationController.Initialize(gameDataManager, enemySpaceshipBuilder);
			gameplayController.WaveChange += formationController.OnWaveChanged;
		}

		private void OnEnemyAdded(Spaceship spaceship)
		{
			spaceship.Destroyed += OnEnemyDestroyed;
			enemySpaceships.Add(spaceship);
		}

		private void OnEnemyDestroyed(Spaceship spaceship)
		{
			spaceship.Destroyed -= OnEnemyDestroyed;
			enemySpaceships.Remove(spaceship);
			if (enemySpaceships.Count == 0)
			{
				AllSpaceshipDestroyed?.Invoke();
			}
		}
	}
}