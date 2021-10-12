using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Weapon
	{
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

		public void SetDurability(int durability)
		{
			this.durability = durability;
		}
	}
}