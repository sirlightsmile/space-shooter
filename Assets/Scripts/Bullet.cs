using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Bullet : PoolObject
	{
		private void FixedUpdate()
		{
			if (IsActive)
			{
				this.transform.Translate(Vector3.forward * Time.fixedDeltaTime);
			}
		}

		public override void OnSpawn() { }

		public override void OnDespawn() { }
	}
}