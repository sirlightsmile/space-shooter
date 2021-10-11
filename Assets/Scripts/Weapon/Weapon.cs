using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public enum AttackType
	{
		Laser,
		Bullet
	}

	public abstract class Weapon
	{
		protected AttackType attackType;
		protected int durability;
		protected int level;
		protected int maxLevel;
		protected int damage;

		public abstract void Attack();

		public void SetDamage(int damage)
		{
			this.damage = damage;
		}

		public void SetLevel(int level)
		{
			this.level = level;
		}

		public void SetMaxLevel(int maxLevel)
		{
			this.maxLevel = maxLevel;
		}

		public void SetAttackType(AttackType attackType)
		{
			this.attackType = attackType;
		}

		public void SetDurability(int durability)
		{
			this.durability = durability;
		}
	}
}