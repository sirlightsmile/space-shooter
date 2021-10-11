using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public enum AttackType
	{
		Laser,
		Bullet
	}

	public class Weapon : MonoBehaviour
	{
		private AttackType attackType;
		private int durability;
		private int level;
		private int maxLevel;
		private float damage;

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