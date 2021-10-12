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

		private void OnCollisionEnter(Collision other)
		{
			string tag = other.transform.tag;
			Debug.Log("Collision with : " + tag);
		}
	}
}