using UnityEngine;

namespace SmileProject.Generic
{
	public abstract class PoolObject : MonoBehaviour
	{
		public bool IsActive { get { return gameObject.activeInHierarchy; } }
		private string poolName;
		private PoolManager poolManager;

		/// <summary>
		/// Set pool name and pool manager as reference for return self
		/// </summary>
		/// <param name="poolName">Pool name</param>
		/// <param name="poolManager">Pool manager</param>
		public void SetPool(string poolName, PoolManager poolManager)
		{
			this.poolName = poolName;
			this.poolManager = poolManager;
		}

		/// <summary>
		/// Return self to pool manager
		/// </summary>
		public virtual void ReturnToPool()
		{
			SetActive(false);
			poolManager.ReturnItem(poolName, this);
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