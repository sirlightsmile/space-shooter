using System;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemyManager
	{
		public event Action AllSpaceshipDestroyed;
		private FormationController formationController;

		public EnemyManager(GameplayController gameplayController, IResourceLoader resourceLoader, GameDataManager gameDataManager, FormationController formationController)
		{
			SpaceshipBuilder enemySpaceshipBuilder = new EnemySpaceshipBuilder(resourceLoader, gameDataManager);
			this.formationController = formationController;
			formationController.Initialize(gameDataManager, enemySpaceshipBuilder);
			gameplayController.WaveChange += formationController.OnWaveChanged;
			gameplayController.Start += OnGameStart;
		}

		private void OnGameStart()
		{

		}
	}
}