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

		public SpaceshipGun(SpaceshipGunModel model)
		{
			this.model = model;
			SetLevel(WeaponInitialLevel);
			SetMaxLevel(model.MaxLevel);
			SetDamage(model.BaseDamage);
			SetAttackSpeed(model.BaseSpeed);

			InitBulletPool();
		}

		//TODO: move this to bullet manager or something.
		private async void InitBulletPool()
		{
			if (!PoolManager.GetInstance().HasPool(model.BulletAsset))
			{
				PoolOptions options = new PoolOptions
				{
					AssetKey = model.BulletAsset,
					PoolName = model.BulletType.ToString(),
					InitialSize = 8,
					CanExtend = true,
					ExtendAmount = 8
				};
				await PoolManager.GetInstance().CreatePool(options);
			}
		}

		public void Shoot()
		{
			Bullet bullet = PoolManager.GetInstance().GetItem<Bullet>(model.BulletType.ToString());
			bullet.transform.SetParent(null);
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