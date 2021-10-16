using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Bullet : PoolObject
	{
		[SerializeField]
		private SpriteRenderer bulletRenderer;
		private int damage;

		public override void OnSpawn() { }

		public override void OnDespawn() { }

		public void SetDamage(int damage)
		{
			this.damage = damage;
		}

		private void FixedUpdate()
		{
			this.transform.Translate(this.transform.up * Time.fixedDeltaTime * 5f);
			if (!bulletRenderer.isVisible)
			{
				ReturnToPool();
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			Spaceship spaceship = other.transform.GetComponent<Spaceship>();
			if (spaceship != null)
			{
				spaceship.GetHit(damage);
			}
			ReturnToPool();
		}
	}
}