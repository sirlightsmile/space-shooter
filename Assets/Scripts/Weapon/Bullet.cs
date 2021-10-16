using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Bullet : PoolObject
	{
		[SerializeField]
		private SpriteRenderer bulletRenderer;
		private PoolManager poolManager;

		public override void OnSpawn() { }

		public override void OnDespawn() { }

		private void FixedUpdate()
		{
			this.transform.Translate(this.transform.up * Time.fixedDeltaTime * 5f);
			if (!bulletRenderer.isVisible)
			{
				ReturnToPool();
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			string tag = other.transform.tag;
			//TODO: implement get hit
			ReturnToPool();
		}
	}
}