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
		private float _pointBlankRange = 2f;

		/// <summary>
		/// Interval between each point-blank shot
		/// </summary>
		private float _pointBlankShotInterval = 0.3f;

		/// <summary>
		/// Number of point-blank shot
		/// </summary>
		private int _pointBlankShot = 6;
		protected SpaceshipGun _pointBlankWeapon;
		private Coroutine _pointBlankCoroutine;
		private int _pointBlankSoundIdRef = -1;

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
			_pointBlankWeapon = weapon;
			_pointBlankWeapon.SetAttackPointTransform(_attackPointTransform);
			await _pointBlankWeapon.Reload();
		}

		/// <summary>
		/// Perform point-blank attack
		/// </summary>
		public async Task PointBlankAttack(Transform target)
		{
			if (_pointBlankWeapon == null)
			{
				Debug.LogAssertion("No point-blank weapon to shoot");
				return;
			}
			IsPerformPointBlank = true;
			_pointBlankSoundIdRef = await PlaySound(GameSoundKeys.Drone);
			_pointBlankCoroutine = StartCoroutine(PointBlankCoroutine(target));
		}

		private IEnumerator PointBlankCoroutine(Transform target)
		{
			Transform parent = transform.parent;
			transform.SetParent(null);
			bool isReachedPoint = false;
			Action onReached = () => { isReachedPoint = true; };

			// move to point-blank position
			MoveToTarget(target, onReached, new Vector2(0, _pointBlankRange), true);
			yield return new WaitUntil(() => isReachedPoint);

			// point-blank shot
			for (int i = 0; i < _pointBlankShot; i++)
			{
				Shoot(_pointBlankWeapon);
				yield return new WaitForSeconds(_pointBlankShotInterval);
			}

			// move back to position
			isReachedPoint = false;
			MoveToTarget(parent.transform, onReached);
			yield return new WaitUntil(() => isReachedPoint);
			transform.SetParent(parent);
			transform.localPosition = Vector2.zero;
			IsPerformPointBlank = false;
			_pointBlankCoroutine = null;
			_pointBlankSoundIdRef = -1;
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			if (_pointBlankCoroutine != null)
			{
				StopCoroutine(_pointBlankCoroutine);
				_pointBlankCoroutine = null;
			}
			if (_pointBlankSoundIdRef >= 0)
			{
				_audioManager?.StopSound(_pointBlankSoundIdRef);
			}
			ReturnToPool();
		}
	}
}
