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

		public EnemyManager(GameplayController gameplayController, FormationController formationController)
		{
			this.formationController = formationController;
			formationController.SpaceshipAdded += OnEnemySpaceshipAdded;
			gameplayController.WaveChange += formationController.OnWaveChanged;
		}

		private void OnEnemySpaceshipAdded(Spaceship spaceship)
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