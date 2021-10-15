using UnityEngine;
using UnityEngine.Events;

namespace SmileProject.SpaceShooter
{
	public abstract class Spaceship : MonoBehaviour, ISpaceship
	{
		public event SpaceshipDestroyed Destroyed;
		public delegate void SpaceshipDestroyed(Spaceship spaceship);
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

		public virtual void Setup(SpaceshipModel spaceshipModel)
		{
			this.SetHP(spaceshipModel.HP);
			this.SetSpeed(spaceshipModel.Speed);
		}

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

		public virtual void SetSpeed(float speed)
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

		protected virtual void ShipDestroy()
		{
			Destroyed?.Invoke(this);
		}
	}
}
