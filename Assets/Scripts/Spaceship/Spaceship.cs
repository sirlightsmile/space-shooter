using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Spaceship : MonoBehaviour
	{
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

		protected SpaceshipGun weapon;

		public virtual void Shoot()
		{
			if (weapon == null)
			{
				Debug.LogAssertion("Spaceship weapon should not be null.");
				return;
			}
			weapon.Shoot();
		}

		public virtual void SetSprite(Sprite sprite)
		{
			this.shipImage.sprite = sprite;
		}

		public virtual void SetHP(int hp)
		{
			this.hp = hp;
		}

		public virtual void SetSpeed(int speed)
		{
			this.speed = speed;
		}

		public virtual void SetWeapon(SpaceshipGun weapon)
		{
			this.weapon = weapon;
			this.weapon.SetAttackPointTransform(attackPointTransform);
		}

		public virtual void GetHit(int damage)
		{
			int result = this.hp - damage;
			this.hp = Mathf.Clamp(result, 0, this.hp);
		}

		protected abstract void ShipDestroy();
	}
}
