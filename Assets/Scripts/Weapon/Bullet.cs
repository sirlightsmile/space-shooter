using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Bullet : PoolObject
	{
		[SerializeField]
		private SpriteRenderer bulletRenderer;

		private void FixedUpdate()
		{
			this.transform.Translate(this.transform.up * Time.fixedDeltaTime * 5f);
			if (!bulletRenderer.isVisible)
			{
				OnInvisible();
			}
		}

		public override void OnSpawn() { }

		public override void OnDespawn() { }

		private void OnCollisionEnter(Collision other)
		{
			string tag = other.transform.tag;
			Debug.Log("Collision with : " + tag);
		}

		private void OnInvisible()
		{
			SetActive(false);
			PoolManager.GetInstance().ReturnItem(poolName, this);
		}
	}
}