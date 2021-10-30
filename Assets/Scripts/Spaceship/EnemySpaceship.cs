using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		public override SpaceshipTag SpaceshipTag { get { return SpaceshipTag.Enemy; } }
		public int DestroyScore { get; private set; }
		public bool IsPerformPointBlank { get; private set; }

		/// <summary>
		/// Point-blank setup range
		/// </summary>
		[SerializeField]
		private float pointBlankRange = 2f;

		/// <summary>
		/// Interval between each point-blank shot
		/// </summary>
		private float pointBlankShotInterval = 0.3f;

		/// <summary>
		/// Number of point-blank shot
		/// </summary>
		private int pointBlankShot = 6;
		protected SpaceshipGun pointBlankWeapon;
		private Coroutine pointBlankCoroutine;
		private int pointBlankSoundIdRef = -1;

		/// <summary>
		/// Set score that player will get when this spaceship destroyed
		/// </summary>
		/// <param name="destroyScore">Score</param>
		public void SetDestroyScore(int destroyScore)
		{
			this.DestroyScore = destroyScore;
		}

		public async Task SetPointBlankWeapon(SpaceshipGun weapon)
		{
			this.pointBlankWeapon = weapon;
			this.pointBlankWeapon.SetAttackPointTransform(attackPointTransform);
			await pointBlankWeapon.Reload();
		}

		/// <summary>
		/// Perform point-blank attack
		/// </summary>
		public async void PointBlankAttack(Transform target)
		{
			if (pointBlankWeapon == null)
			{
				Debug.LogAssertion("No point-blank weapon to shoot");
				return;
			}
			IsPerformPointBlank = true;
			pointBlankSoundIdRef = await PlaySound(GameSoundKeys.Drone);
			pointBlankCoroutine = StartCoroutine(PointBlankCoroutine(target));
		}

		private IEnumerator PointBlankCoroutine(Transform target)
		{
			Transform parent = transform.parent;
			transform.SetParent(null);
			bool isReachedPoint = false;
			Action onReached = () => { isReachedPoint = true; };

			// move to point-blank position
			MoveToTarget(target, onReached, new Vector2(0, pointBlankRange), true);
			yield return new WaitUntil(() => isReachedPoint);

			// point-blank shot
			for (int i = 0; i < pointBlankShot; i++)
			{
				Shoot(pointBlankWeapon);
				yield return new WaitForSeconds(pointBlankShotInterval);
			}

			// move back to position
			isReachedPoint = false;
			MoveToTarget(parent.transform, onReached);
			yield return new WaitUntil(() => isReachedPoint);
			transform.SetParent(parent);
			transform.localPosition = Vector2.zero;
			IsPerformPointBlank = false;
			pointBlankCoroutine = null;
			pointBlankSoundIdRef = -1;
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			if (pointBlankCoroutine != null)
			{
				StopCoroutine(pointBlankCoroutine);
				pointBlankCoroutine = null;
			}
			if (pointBlankSoundIdRef >= 0)
			{
				audioManager?.StopSound(pointBlankSoundIdRef);
			}
			ReturnToPool();
		}
	}
}
