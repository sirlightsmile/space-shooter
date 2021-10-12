using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class SpaceShip : MonoBehaviour
	{
		public Weapon Weapon { get; private set; }
		public int HP { get { return hp; } }

		[SerializeField]
		protected int hp;

		[SerializeField]
		protected float speed;

		[SerializeField]
		protected float atk;

		[SerializeField]
		protected Transform attackPointTransform;

		public SpaceShip(int hp, float speed, float atk)
		{
			this.hp = hp;
			this.speed = speed;
			this.atk = atk;
		}

		public void SetWeapon(Weapon weapon)
		{
			this.Weapon = weapon;
			this.Weapon.SetAttackPointTransform(attackPointTransform);
		}

		public virtual void GetHit(int damage)
		{
			int result = this.hp - damage;
			this.hp = Mathf.Clamp(result, 0, this.hp);
		}

		protected abstract void ShipDestroy();
	}
}
