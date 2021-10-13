using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class WeaponFactory
	{
		private GameDataManager gameDataManager;

		public WeaponFactory(GameDataManager gameDataManager)
		{
			this.gameDataManager = gameDataManager;
		}

		public SpaceshipGun CreateSpaceshipGun(SpaceshipGunModel model)
		{
			SpaceshipGun gun = new SpaceshipGun(model);
			return gun;
		}

		public SpaceshipGun CreateRandomSpaceshipGun()
		{
			var spaceshipGuns = this.gameDataManager.GetSpaceshipGunModels();
			int randomIndex = Random.Range(0, spaceshipGuns.Length);
			SpaceshipGunModel randomModel = spaceshipGuns[randomIndex];
			SpaceshipGun randomGun = new SpaceshipGun(randomModel);
			return randomGun;
		}
	}
}