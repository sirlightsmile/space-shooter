
using System;
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceshipBuilder : SpaceshipBuilder
	{
		private const string ASSET_PREFIX = "SpaceshipSprites/";
		private const string ENEMY_PREFAB_KEY = "EnemyPrefab";
		private const int ENEMY_INITIAL_POOL_SIZE = 10;
		private GameDataManager _gameDataManager;
		private AudioManager _audioManager;
		private WeaponFactory _weaponFactory;

		public EnemySpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader)
		{
			_gameDataManager = gameDataManager;
			_audioManager = audioManager;
			_weaponFactory = weaponFactory;
		}

		public async Task SetupSpaceshipPool(PoolManager poolManager)
		{
			await SetupPool(poolManager, ENEMY_PREFAB_KEY, ENEMY_INITIAL_POOL_SIZE);
		}

		public async Task<EnemySpaceship> BuildEnemySpaceship(EnemySpaceshipModel model)
		{
			var spaceship = await BuildSpaceship<EnemySpaceship, EnemySpaceshipModel>(ENEMY_PREFAB_KEY, model);
			string weaponId = model.BasicWeaponId;
			if (!String.IsNullOrEmpty(weaponId))
			{
				SpaceshipGun weapon = _weaponFactory.CreateSpaceshipGunById(weaponId);
				await spaceship.SetWeapon(weapon);
			}
			//TODO: change how to get weapon id later
			SpaceshipGun pointBlankWeapon = _weaponFactory.CreateSpaceshipGunById("sg02");
			await spaceship.SetPointBlankWeapon(pointBlankWeapon);
			spaceship.SetDestroyScore(model.Score);
			spaceship.SetSounds(_audioManager, GameSoundKeys.Hit, GameSoundKeys.Explosion);
			return spaceship;
		}

		public async override Task<Spaceship> BuildSpaceshipById(string id)
		{
			EnemySpaceshipModel model = _gameDataManager.GetEnemySpaceshipModelById(id);
			return await BuildEnemySpaceship(model);
		}

		protected override string GetAssetPrefix()
		{
			return ASSET_PREFIX;
		}
	}
}