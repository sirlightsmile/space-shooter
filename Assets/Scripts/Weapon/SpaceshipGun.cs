using System;
using UnityEngine;

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
			SetLevel(WeaponInitialLevel);
			SetMaxLevel(model.MaxLevel);
			SetDamage(model.BaseDamage);
			SetAttackSpeed(model.BaseSpeed);
		}

		public void Shoot()
		{
			Bullet bullet = PoolManager.GetInstance().GetItem<Bullet>(model.BulletAsset);
			bullet.transform.SetParent(null);
			bullet.transform.position = attackPointTransform.transform.position;
			bullet.transform.rotation = attackPointTransform.transform.rotation;
			bullet.gameObject.SetActive(true);
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