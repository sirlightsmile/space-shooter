﻿using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Spaceship : MonoBehaviour
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

		[SerializeField]
		/// <summary>
		/// Move smooth time
		/// </summary>
		private float smoothTime = 0.5f;

		[SerializeField]
		/// <summary>
		/// Reach target approximate distance
		/// </summary>
		private float approximate = 0.01f;

		private Coroutine MoveCoroutine;

		public virtual void Setup<T>(T spaceshipModel) where T : SpaceshipModel
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

		public virtual async Task SetWeapon(SpaceshipGun newWeapon)
		{
			await newWeapon.Reload();
			this.weapon = newWeapon;
			this.weapon.SetAttackPointTransform(attackPointTransform);
		}

		public virtual void GetHit(int damage)
		{
			int result = this.hp - damage;
			this.hp = Mathf.Clamp(result, 0, this.hp);
		}

		public virtual void SetPosition(Vector2 position)
		{
			this.transform.position = position;
		}

		public virtual void MoveToTarget(Vector2 targetPos)
		{
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
			}

			MoveCoroutine = StartCoroutine(MoveToTargetCoroutine(targetPos));
		}

		protected virtual void ShipDestroy()
		{
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
				MoveCoroutine = null;
			}
			Destroyed?.Invoke(this);
		}

		protected virtual void OnTargetReached()
		{
			Debug.Log("On target reached");
		}

		private IEnumerator MoveToTargetCoroutine(Vector2 targetPos)
		{
			Vector2 currentPos = this.transform.position;
			float distance = Vector2.Distance(currentPos, targetPos);
			Vector2 velocity = Vector3.zero;

			while (distance > approximate)
			{
				Vector2 dampPos = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime, speed);
				SetPosition(dampPos);
				currentPos = this.transform.position;
				distance = (targetPos - currentPos).magnitude;
				yield return null;
			}
			// snap
			SetPosition(targetPos);
			OnTargetReached();
			MoveCoroutine = null;
		}
	}
}
