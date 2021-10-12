using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class SpaceShipGun : Weapon
	{
		public BulletType BulletType { get; private set; }
		private SpaceShipGunModel model;

		public SpaceShipGun(SpaceShipGunModel model)
		{
			this.BulletType = model.BulletType;
			SetLevel(WeaponInitialLevel);
			SetMaxLevel(model.MaxLevel);
			SetBulletType(model.BulletType);
			SetDamage(model.BaseDamage);
			SetAttackSpeed(model.BaseSpeed);
		}

		public void Shoot()
		{
			//TODO: implement
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

		private void SetBulletType(BulletType attackType)
		{
			this.BulletType = attackType;
		}
	}
}