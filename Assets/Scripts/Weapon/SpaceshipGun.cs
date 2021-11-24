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
		private SpaceshipGunModel _model;
		private PoolManager _poolManager;
		private AudioManager _audioManager;
		private SoundKeys _shootSound;

		public SpaceshipGun(SpaceshipGunModel model, PoolManager poolManager)
		{
			_model = model;
			_poolManager = poolManager;
			SetLevel(WEAPON_INITIAL_LEVEL);
			SetMaxLevel(model.MaxLevel);
			SetDamage(model.BaseDamage);
			SetAttackSpeed(model.BaseSpeed);
		}

		public async Task Reload()
		{
			string poolName = _model.BulletType.ToString();
			if (!_poolManager.HasPool(poolName))
			{
				PoolOptions options = new PoolOptions
				{
					//TODO: adjust size
					AssetKey = _model.BulletAsset,
					PoolName = poolName,
					InitialSize = 10,
					CanExtend = true,
					ExtendAmount = 10
				};
				await _poolManager.CreatePool<Bullet>(options);
			}
		}

		public void Shoot(Spaceship owner)
		{
			Bullet bullet = _poolManager.GetItem<Bullet>(_model.BulletType.ToString());
			Transform attackPoint = _attackPointTransform.transform;
			bullet.SetPosition(attackPoint.position).SetRotation(attackPoint.rotation).SetDamage(_damage).SetOwner(owner);
			bullet.SetActive(true);
			var _ = PlayShootSound();
		}

		public void LevelUp(int addLevel = 1)
		{
			int addedLevel = _level + addLevel;
			int targetLevel = Mathf.Clamp(addedLevel, _level, _maxLevel);
			if (_level != targetLevel)
			{
				SetLevel(targetLevel);
				UpdateStatus();
			}
		}

		public void SetSounds(AudioManager audioManager, SoundKeys shootSound)
		{
			_audioManager = audioManager;
			_shootSound = shootSound;
		}

		private async Task PlayShootSound()
		{
			if (_audioManager != null)
			{
				await _audioManager.PlaySound(_shootSound);
			}
		}

		private void UpdateStatus()
		{
			int currentLevel = _level;
			int newDamage = _model.BaseDamage + (currentLevel * _model.DamageIncrement);
			int newAttackSpeed = _model.BaseSpeed + (currentLevel * _model.SpeedIncrement);
			SetDamage(newDamage);
			SetAttackSpeed(newAttackSpeed);
		}
	}
}