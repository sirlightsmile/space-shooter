using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SmileProject.SpaceShooter
{
	/// <summary>
	/// Common spaceship gun. Infinity bullet
	/// </summary>
	public class SpaceshipGun : Weapon
	{
		private SpaceshipGunModel model;
		private PoolManager poolManager;

		public SpaceshipGun(SpaceshipGunModel model, PoolManager poolManager)
		{
			this.model = model;
			this.poolManager = poolManager;
			SetLevel(WeaponInitialLevel);
			SetMaxLevel(model.MaxLevel);
			SetDamage(model.BaseDamage);
			SetAttackSpeed(model.BaseSpeed);
		}

		public async void Reload()
		{
			if (!poolManager.HasPool(model.BulletAsset))
			{
				PoolOptions options = new PoolOptions
				{
					//TODO: adjust size
					AssetKey = model.BulletAsset,
					PoolName = model.BulletType.ToString(),
					InitialSize = 10,
					CanExtend = true,
					ExtendAmount = 10
				};
				await PoolManager.GetInstance().CreatePool(options);
			}
		}

		public void Shoot()
		{
			Bullet bullet = poolManager.GetItem<Bullet>(model.BulletType.ToString());
			bullet.transform.position = attackPointTransform.transform.position;
			bullet.transform.rotation = attackPointTransform.transform.rotation;
			bullet.SetActive(true);
		}

		public void LevelUp(int addLevel = 1)
		{
			int addedLevel = this.level + addLevel;
			int targetLevel = Mathf.Clamp(addedLevel, this.level, this.maxLevel);
			if (this.level != targetLevel)
			{
				SetLevel(targetLevel);
				UpdateStatus();
			}
		}

		private void UpdateStatus()
		{
			int currentLevel = this.level;
			int newDamage = model.BaseDamage + (currentLevel * model.DamageIncrement);
			int newAttackSpeed = model.BaseSpeed + (currentLevel * model.SpeedIncrement);
			SetDamage(newDamage);
			SetAttackSpeed(newAttackSpeed);
		}
	}
}