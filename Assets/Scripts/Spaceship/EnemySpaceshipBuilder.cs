
using System;
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
		private WeaponFactory weaponFactory;

		public EnemySpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader)
		{
			this.gameDataManager = gameDataManager;
			this.audioManager = audioManager;
			this.weaponFactory = weaponFactory;
		}

		public async Task<EnemySpaceship> BuildEnemySpaceship(EnemySpaceshipModel model)
		{
			var spaceship = await BuildSpaceship<EnemySpaceship, EnemySpaceshipModel>(enemyPrefabKey, model);
			string weaponId = model.BasicWeaponId;
			if (!String.IsNullOrEmpty(weaponId))
			{
				SpaceshipGun weapon = weaponFactory.CreateSpaceshipGunById(weaponId);
				await spaceship.SetWeapon(weapon);
			}
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