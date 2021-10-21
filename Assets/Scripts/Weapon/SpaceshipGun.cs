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
		private GameSoundKeys shootSound;

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
				await poolManager.CreatePool<Bullet>(options);
			}
		}

		public void Shoot()
		{
			Bullet bullet = poolManager.GetItem<Bullet>(model.BulletType.ToString());
			bullet.SetParent(null);
			bullet.transform.position = attackPointTransform.transform.position;
			bullet.transform.rotation = attackPointTransform.transform.rotation;
			bullet.SetDamage(damage);
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

		public void SetSound(AudioManager audioManager, GameSoundKeys shootSound)
		{
			this.audioManager = audioManager;
			this.shootSound = shootSound;
		}

		private async void PlayShootSound()
		{
			if (audioManager != null && shootSound != null)
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