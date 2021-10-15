using System.Collections;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
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
		private int destroyScore;

		public void SetDestroyScore(int destroyScore)
		{
			this.destroyScore = destroyScore;
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
			if (MoveCoroutine != null)
			{
				StopCoroutine(MoveCoroutine);
				MoveCoroutine = null;
			}
		}

		private IEnumerator MoveToTargetCoroutine(Vector2 targetPos)
		{
			Vector2 currentPos = this.transform.position;
			float distance = Vector2.Distance(currentPos, targetPos);
			Vector2 velocity = Vector3.zero;

			while (distance > approximate)
			{
				transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime, speed);
				currentPos = this.transform.position;
				distance = (targetPos - currentPos).magnitude;
				yield return null;
			}
			// snap
			this.transform.position = targetPos;
			OnTargetReached();
			MoveCoroutine = null;
		}

		private void OnTargetReached()
		{
			Debug.Log("On target reached");
			//TODO: rotate down
		}
	}
}
