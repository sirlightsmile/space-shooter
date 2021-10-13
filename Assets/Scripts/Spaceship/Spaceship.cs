using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Spaceship : MonoBehaviour
	{
		public Weapon Weapon { get; private set; }
		public int HP { get { return hp; } }
		protected float width { get { return shipImage.bounds.size.x * shipImage.sprite.pixelsPerUnit; } }
		protected float height { get { return shipImage.bounds.size.y * shipImage.sprite.pixelsPerUnit; } }

		[SerializeField]
		protected int hp;

		[SerializeField]
		protected float speed;

		[SerializeField]
		protected Transform attackPointTransform;

		[SerializeField]
		protected SpriteRenderer shipImage;

		public Spaceship(int hp, float speed)
		{
			this.hp = hp;
			this.speed = speed;
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
