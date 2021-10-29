using System;
using System.Collections;
using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Spaceship : MonoBehaviour
	{
		/// <summary>
		/// Invoke when got hit <Attacker, Defender>
		/// </summary>
		public event Action<Spaceship, Spaceship> GotHit;

		/// <summary>
		/// Invoke when spaceship destroyed
		/// </summary>
		public event SpaceshipDestroyed Destroyed;
		public delegate void SpaceshipDestroyed(Spaceship spaceship);

		public abstract SpaceshipTag SpaceshipTag
		{
			get;
		}

		public int HP { get; protected set; }

		[SerializeField]
		protected float speed;

		[SerializeField]
		protected Transform attackPointTransform;

		[SerializeField]
		protected SpriteRenderer shipImage;

		protected float width { get { return shipImage.bounds.size.x * shipImage.sprite.pixelsPerUnit; } }
		protected float height { get { return shipImage.bounds.size.y * shipImage.sprite.pixelsPerUnit; } }

		protected SpaceshipGun weapon;
		protected AudioManager audioManager;
		protected SoundKeys getHitSound;
		protected SoundKeys destroyedSound;

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

		/// <summary>
		/// Shoot from spaceship weapon
		/// </summary>
		/// <param name="customWeapon">Custom weapon to shoot. If 'null' will use weapon attached to spaceship</param>
		public virtual void Shoot(SpaceshipGun customWeapon = null)
		{
			if (weapon == null)
			{
				Debug.LogAssertion("Spaceship weapon should not be null.");
				return;
			}
			weapon.Shoot(this);
		}

		public virtual void SetSprite(Sprite sprite)
		{
			this.shipImage.sprite = sprite;
		}

		public virtual void SetHP(int hp)
		{
			this.HP = hp;
		}

		public virtual bool IsBroken()
		{
			return HP <= 0;
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

		/// <summary>
		/// Get hit by bullet
		/// </summary>
		/// <param name="damage">damage or weapon shot that bullet</param>
		/// <param name="attacker">attacker</param>
		public virtual void GetHit(int damage, Spaceship attacker)
		{
			int result = HP - damage;
			HP = Mathf.Clamp(result, 0, this.HP);
			GotHit?.Invoke(attacker, this);
			PlaySound(getHitSound);
			if (IsBroken())
			{
				ShipDestroy();
			}
		}

		public virtual void SetPosition(Vector2 position)
		{
			this.transform.position = position;
		}

		/// <summary>
		/// Start coroutine for move to target position (2d)
		/// </summary>
		/// <param name="targetPos">Target position</param>
		/// <param name="reachedCallback">Callback when reached target position</param>
		public virtual void MoveToTarget(Vector2 targetPos, Action reachedCallback = null)
		{
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
			}

			MoveCoroutine = StartCoroutine(MoveToTargetCoroutine(targetPos, reachedCallback));
		}

		public virtual void SetSounds(AudioManager audioManager, SoundKeys getHitSound, SoundKeys destroyedSound)
		{
			this.audioManager = audioManager;
			this.getHitSound = getHitSound;
			this.destroyedSound = destroyedSound;
		}

		protected async void PlaySound(SoundKeys soundKey)
		{
			if (audioManager != null && soundKey != null)
			{
				await audioManager.PlaySound(soundKey);
			}
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

		private IEnumerator MoveToTargetCoroutine(Vector2 targetPos, Action reachedCallback)
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
				yield return new WaitForFixedUpdate();
			}
			// snap
			SetPosition(targetPos);
			reachedCallback?.Invoke();
			MoveCoroutine = null;
		}
	}
}
