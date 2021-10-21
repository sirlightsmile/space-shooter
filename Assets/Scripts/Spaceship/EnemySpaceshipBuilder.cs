
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceshipBuilder : SpaceshipBuilder
	{
		private const string assetPrefix = "SpaceshipSprites/";
		private const string enemyPrefabKey = "EnemyPrefab";
		private GameDataManager gameDataManager;
		private AudioManager audioManager;

		public EnemySpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, AudioManager audioManager) : base(resourceLoader)
		{
			this.gameDataManager = gameDataManager;
			this.audioManager = audioManager;
		}

		public async Task<EnemySpaceship> BuildEnemySpaceship(EnemySpaceshipModel model)
		{
			var spaceship = await BuildSpaceship<EnemySpaceship, EnemySpaceshipModel>(enemyPrefabKey, model);
			spaceship.SetDestroyScore(model.Score);
			spaceship.SetSounds(audioManager, GameSoundKeys.Explosion);
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