
using System;
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class PlayerSpaceshipBuilder : SpaceshipBuilder
	{
		private const string assetPrefix = "SpaceshipSprites/";
		private const string prefabKey = "PlayerPrefab";
		private GameDataManager gameDataManager;
		private WeaponFactory weaponFactory;
		private AudioManager audioManager;

		public PlayerSpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader)
		{
			this.gameDataManager = gameDataManager;
			this.weaponFactory = weaponFactory;
			this.audioManager = audioManager;
		}

		public async Task<PlayerSpaceship> BuildPlayerSpaceship(SpaceshipModel model)
		{
			var spaceship = await BuildSpaceship<PlayerSpaceship, SpaceshipModel>(prefabKey, model);
			string weaponId = model.BasicWeaponId;
			SpaceshipGun weapon = !String.IsNullOrEmpty(weaponId) ? weaponFactory.CreateSpaceshipGunById(weaponId) : weaponFactory.CreateRandomSpaceshipGun();
			await spaceship.SetWeapon(weapon);
			spaceship.SetSounds(audioManager, GameSoundKeys.PlayerExplosion);
			return spaceship;
		}

		public async override Task<Spaceship> BuildSpaceshipById(string id)
		{
			SpaceshipModel model = gameDataManager.GetPlayerSpaceshipModelById(id);
			return await BuildPlayerSpaceship(model);
		}

		public async Task<PlayerSpaceship> BuildRandomSpaceship()
		{
			SpaceshipModel[] models = gameDataManager.GetPlayerSpaceshipModels();
			int randomIndex = UnityEngine.Random.Range(0, models.Length);
			SpaceshipModel randomModel = models[randomIndex];
			return await BuildPlayerSpaceship(randomModel);
		}

		protected override string GetAssetPrefix()
		{
			return assetPrefix;
		}
	}
}