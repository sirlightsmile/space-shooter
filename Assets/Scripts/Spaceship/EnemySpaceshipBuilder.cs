
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceshipBuilder : SpaceshipBuilder
	{
		private const string enemyPrefabKey = "EnemyPrefab";
		public EnemySpaceshipBuilder(IResourceLoader resourceLoader) : base(resourceLoader)
		{
		}

		public async Task<EnemySpaceship> BuildEnemySpaceship(EnemySpaceshipModel model)
		{
			var spaceship = await BuildSpaceship<EnemySpaceship, EnemySpaceshipModel>(enemyPrefabKey, model);
			spaceship.SetDestroyScore(model.Score);
			return spaceship;
		}
	}
}