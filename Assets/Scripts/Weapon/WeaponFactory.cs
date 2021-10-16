using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class WeaponFactory
	{
		private GameDataManager gameDataManager;
		private PoolManager poolManager;

		public WeaponFactory(GameDataManager gameDataManager, PoolManager poolManager)
		{
			this.gameDataManager = gameDataManager;
			this.poolManager = poolManager;
		}

		public SpaceshipGun CreateSpaceshipGun(SpaceshipGunModel model)
		{
			SpaceshipGun gun = new SpaceshipGun(model, poolManager);
			return gun;
		}

		public SpaceshipGun CreateRandomSpaceshipGun()
		{
			var spaceshipGuns = this.gameDataManager.GetSpaceshipGunModels();
			int randomIndex = Random.Range(0, spaceshipGuns.Length);
			SpaceshipGunModel randomModel = spaceshipGuns[randomIndex];
			SpaceshipGun randomGun = CreateSpaceshipGun(randomModel);
			return randomGun;
		}
	}
}