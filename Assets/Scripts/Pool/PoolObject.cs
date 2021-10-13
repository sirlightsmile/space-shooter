using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class PoolObject : MonoBehaviour
	{
		public bool IsActive { get { return gameObject.activeInHierarchy; } }
		protected string poolName;

		public void SetPoolName(string poolName)
		{
			this.poolName = poolName;
		}

		public virtual void SetAction(bool isActive)
		{
			this.gameObject.SetActive(isActive);
		}

		public abstract void OnSpawn();

		public abstract void OnDespawn();
	}
}