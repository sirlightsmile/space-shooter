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

		public SpaceShip(int hp, float speed, float atk)
		{
			this.hp = hp;
			this.speed = speed;
			this.atk = atk;
		}

		public void SetWeapon(Weapon weapon)
		{
			this.Weapon = weapon;
		}

		public abstract void GetHit(int damage);

		protected abstract void ShipDestroy();
	}
}
