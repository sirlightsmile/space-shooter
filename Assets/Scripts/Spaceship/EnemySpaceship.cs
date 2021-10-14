using System.Collections;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public enum MovePattern
	{
		Straight,
		Zigzag,
		Curve
	}
	public class EnemySpaceship : Spaceship
	{
		public MovePattern pattern;

		public Coroutine MoveCoroutine;

		//TODO: implement setup enemy ship from enemy data
		public void Setup()
		{
			SetHP(2);
			SetSpeed(5);
			// SetWeapon(2);
			// SetSprite()
		}


		public void MoveToTarget(Vector3 targetPos)
		{
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
			}

			MoveCoroutine = StartCoroutine(MoveToTargetCoroutine(targetPos));
		}

		protected override void ShipDestroy()
		{
			throw new System.NotImplementedException();
		}

		private IEnumerator MoveToTargetCoroutine(Vector3 targetPos)
		{
			float startTime = Time.time;
			float duration = 1f; // formation speed
			while (Time.time - startTime < duration)
			{
				this.transform.LookAt(targetPos);
				this.transform.position = Vector3.Lerp(this.transform.position, targetPos, (Time.time - startTime) / duration);
				yield return new WaitForFixedUpdate();
			}
			OnTargetReached();
			MoveCoroutine = null;
		}

		private void Update()
		{
		}

		private void OnTargetReached()
		{
			//TODO: rotate down
		}
	}
}
