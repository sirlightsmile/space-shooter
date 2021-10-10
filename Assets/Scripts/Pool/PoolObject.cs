using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class PoolObject : MonoBehaviour
	{
		public bool IsActive { get; private set; }

		public virtual void SetAction(bool isActive)
		{
			this.IsActive = isActive;
			this.gameObject.SetActive(isActive);
		}

		public abstract void OnSpawn();

		public abstract void OnDespawn();
	}
}