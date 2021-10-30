using System;
using System.Collections;
using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class Spaceship : PoolObject
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

		private const string idleAnimState = "Idle";
		private const string getHitAnimState = "GetHit";

		[SerializeField]
		protected float speed;

		[SerializeField]
		protected Transform attackPointTransform;

		[SerializeField]
		protected SpriteRenderer shipImage;

		protected float width { get { return shipImage?.bounds.size.x * shipImage?.sprite.pixelsPerUnit ?? 0; } }
		protected float height { get { return shipImage?.bounds.size.y * shipImage?.sprite.pixelsPerUnit ?? 0; } }

		protected SpaceshipGun weapon;
		protected AudioManager audioManager;
		protected SoundKeys getHitSound;
		protected SoundKeys destroyedSound;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		/// <summary>
		/// Move smooth time
		/// </summary>
		private float smoothTime = 0.1f;

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
			if (weapon == null && customWeapon == null)
			{
				Debug.LogAssertion("Spaceship weapon should not be null.");
				return;
			}
			if (customWeapon != null)
			{
				customWeapon.Shoot(this);
			}
			else
			{
				weapon.Shoot(this);
			}
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
		public virtual async void GetHit(int damage, Spaceship attacker)
		{
			int result = HP - damage;
			HP = Mathf.Clamp(result, 0, this.HP);
			GotHit?.Invoke(attacker, this);

			if (IsBroken())
			{
				ShipDestroy();
			}
			else
			{
				PlayGetHitAnimation();
				await PlaySound(getHitSound);
			}
		}

		public virtual void SetPosition(Vector2 position)
		{
			this.transform.position = position;
		}

		/// <summary>
		/// Start coroutine for move to target transform (2d)
		/// </summary>
		/// <param name="target">Target transform</param>
		/// <param name="reachedCallback">Callback when reached target position</param>
		/// <param name="offset">target position offset</param>
		/// <param name="useStartPosition">if true, will not update target position by transform but use start position</param>
		public virtual void MoveToTarget(Transform target, Action reachedCallback = null, Vector2? offset = null, bool useStartPosition = false)
		{
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
			}

			MoveCoroutine = StartCoroutine(MoveToTargetCoroutine(target, reachedCallback, offset, useStartPosition));
		}

		public virtual void SetSounds(AudioManager audioManager, SoundKeys getHitSound, SoundKeys destroyedSound)
		{
			this.audioManager = audioManager;
			this.getHitSound = getHitSound;
			this.destroyedSound = destroyedSound;
		}

		public override void OnSpawn()
		{
		}

		public override void OnDespawn()
		{
			SetSprite(null);
			weapon = null;
		}

		public async Task<int> PlaySound(SoundKeys soundKey)
		{
			if (audioManager != null && soundKey != null)
			{
				return await audioManager.PlaySound(soundKey);
			}
			return -1;
		}

		protected virtual async void ShipDestroy()
		{
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
				MoveCoroutine = null;
			}
			Destroyed?.Invoke(this);
			await PlaySound(destroyedSound);
		}

		private void OnEnable()
		{
			ResetAnimation();
		}

		private IEnumerator MoveToTargetCoroutine(Transform target, Action reachedCallback, Vector2? offset = null, bool useStartPosition = false)
		{
			Vector2 currentPos = this.transform.position;
			Vector2 startTargetPos = target.position;
			float distance = Vector2.Distance(currentPos, startTargetPos);
			Vector2 velocity = Vector3.zero;

			Func<Vector2> getTargetPos = () =>
			{
				Vector2 targetPos = useStartPosition ? startTargetPos : (Vector2)target.position;
				targetPos.x += offset?.x ?? 0;
				targetPos.y += offset?.y ?? 0;
				return targetPos;
			};

			while (distance > approximate)
			{
				Vector2 targetPos = getTargetPos();
				Vector2 dampPos = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime, speed);
				SetPosition(dampPos);
				currentPos = this.transform.position;
				distance = (targetPos - currentPos).magnitude;
				yield return new WaitForFixedUpdate();
			}

			// snap
			SetPosition(getTargetPos());
			reachedCallback?.Invoke();
			MoveCoroutine = null;
		}

		private void PlayGetHitAnimation()
		{
			animator.Play(getHitAnimState);
		}

		private void ResetAnimation()
		{
			animator.Play(idleAnimState);
		}
	}
}
