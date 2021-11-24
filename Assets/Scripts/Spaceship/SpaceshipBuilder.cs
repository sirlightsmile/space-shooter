using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class SpaceshipBuilder : BaseSpaceshipBuilder
	{
		protected const string ASSET_PREFIX = "SpaceshipSprites/";

		protected GameDataManager _gameDataManager;
		protected AudioManager _audioManager;
		protected WeaponFactory _weaponFactory;

		public SpaceshipBuilder(IResourceLoader resourceLoader, GameDataManager gameDataManager, WeaponFactory weaponFactory, AudioManager audioManager) : base(resourceLoader)
		{
			_gameDataManager = gameDataManager;
			_audioManager = audioManager;
			_weaponFactory = weaponFactory;
		}

		public override Task<Spaceship> BuildSpaceshipById(string id)
		{
			throw new System.NotImplementedException();
		}

		protected override string GetAssetPrefix()
		{
			return ASSET_PREFIX;
		}
	}
}