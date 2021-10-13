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

		public virtual void SetActive(bool isActive)
		{
			this.gameObject.SetActive(isActive);
		}

		public virtual void SetParent(Transform parent)
		{
			this.transform.SetParent(parent);
		}

		public abstract void OnSpawn();

		public abstract void OnDespawn();
	}
}