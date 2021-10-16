
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceshipBuilder : SpaceshipBuilder
	{
		private const string assetPrefix = "SpaceshipSprites/";
		private const string enemyPrefabKey = "EnemyPrefab";
		private GameDataManager gameDataManager;

		public EnemySpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager) : base(resourceLoader)
		{
			this.gameDataManager = gameDataManager;
		}

		public async Task<EnemySpaceship> BuildEnemySpaceship(EnemySpaceshipModel model)
		{
			var spaceship = await BuildSpaceship<EnemySpaceship, EnemySpaceshipModel>(enemyPrefabKey, model);
			spaceship.SetDestroyScore(model.Score);
			return spaceship;
		}

		public async override Task<Spaceship> BuildSpaceshipById(string id)
		{
			EnemySpaceshipModel model = gameDataManager.GetEnemySpaceshipModelById(id);
			return await BuildEnemySpaceship(model);
		}

		protected override string GetAssetPrefix()
		{
			return assetPrefix;
		}
	}
}