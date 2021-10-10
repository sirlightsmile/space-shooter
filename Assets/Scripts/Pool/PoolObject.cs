using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class PoolObject : MonoBehaviour
	{
		public bool IsActive { get { return gameObject.activeInHierarchy; } }

		public virtual void SetAction(bool isActive)
		{
			this.gameObject.SetActive(isActive);
		}

		public abstract void OnSpawn();

		public abstract void OnDespawn();
	}
}