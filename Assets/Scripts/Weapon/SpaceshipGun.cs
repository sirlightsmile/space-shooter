using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	/// <summary>
	/// Common spaceship gun. Infinity bullet
	/// </summary>
	public class SpaceshipGun : Weapon
	{
		private SpaceshipGunModel model;
		private PoolManager poolManager;
		private AudioManager audioManager;
		private SoundKeys shootSound;

		public SpaceshipGun(SpaceshipGunModel model, PoolManager poolManager)
		{
			this.model = model;
			this.poolManager = poolManager;
			SetLevel(WeaponInitialLevel);
			SetMaxLevel(model.MaxLevel);
			SetDamage(model.BaseDamage);
			SetAttackSpeed(model.BaseSpeed);
		}

		public async Task Reload()
		{
			string poolName = model.BulletType.ToString();
			if (!poolManager.HasPool(poolName))
			{
				PoolOptions options = new PoolOptions
				{
					//TODO: adjust size
					AssetKey = model.BulletAsset,
					PoolName = poolName,
					InitialSize = 10,
					CanExtend = true,
					ExtendAmount = 10
				};
				await poolManager.CreatePool<Bullet>(options);
			}
		}

		public void Shoot(Spaceship owner)
		{
			Bullet bullet = poolManager.GetItem<Bullet>(model.BulletType.ToString());
			bullet.SetParent(null);
			bullet.transform.position = attackPointTransform.transform.position;
			bullet.transform.rotation = attackPointTransform.transform.rotation;
			bullet.SetDamage(damage);
			bullet.SetOwner(owner);
			bullet.SetActive(true);
			PlayShootSound();
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

		public void SetSounds(AudioManager audioManager, SoundKeys shootSound)
		{
			this.audioManager = audioManager;
			this.shootSound = shootSound;
		}

		private async void PlayShootSound()
		{
			if (audioManager != null)
			{
				await audioManager.PlaySound(shootSound);
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