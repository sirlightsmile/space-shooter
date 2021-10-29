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
		public void PointBlankAttack(Vector2 targetPos)
		{
			if (pointBlankWeapon == null)
			{
				Debug.LogAssertion("No point-blank weapon to shoot");
				return;
			}
			IsPerformPointBlank = true;
			pointBlankCoroutine = StartCoroutine(PointBlankCoroutine(targetPos));
		}

		private IEnumerator PointBlankCoroutine(Vector2 targetPos)
		{
			Vector2 startPosition = this.transform.position;
			Vector2 pointBlankPos = new Vector2(targetPos.x, targetPos.y + pointBlankRange);
			bool isReachedPoint = false;
			Action onReached = () => { isReachedPoint = true; };

			// move to point-blank position
			MoveToTarget(pointBlankPos, onReached);
			yield return new WaitUntil(() => isReachedPoint);

			// point-blank shot
			for (int i = 0; i < pointBlankShot; i++)
			{
				Shoot(pointBlankWeapon);
				yield return new WaitForSeconds(pointBlankShotInterval);
			}

			// move back to position
			isReachedPoint = false;
			MoveToTarget(startPosition, onReached);
			yield return new WaitUntil(() => isReachedPoint);

			IsPerformPointBlank = false;
			pointBlankCoroutine = null;
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			PlaySound(destroyedSound);
			//TODO: make it pool
			Destroy(this.gameObject);
		}
	}
}
