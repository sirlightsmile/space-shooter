using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Weapon
	{
		public const int WeaponInitialLevel = 1;

		protected int level;
		protected int maxLevel;
		protected int damage;
		protected float attackSpeed;

		protected void SetDamage(int damage)
		{
			this.damage = damage;
		}

		protected void SetLevel(int level)
		{
			this.level = level;
		}

		protected void SetMaxLevel(int maxLevel)
		{
			this.maxLevel = maxLevel;
		}

		protected void SetAttackSpeed(int speed)
		{
			this.attackSpeed = speed;
		}
	}
}