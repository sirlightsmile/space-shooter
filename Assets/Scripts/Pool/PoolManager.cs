using System.Collections.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	[System.Serializable]
	public class PoolOptions
	{
		/// <summary>
		/// Pool name
		/// </summary>
		public string PoolName;

		/// <summary>
		/// Number of initial object when start
		/// </summary>
		public int InitialPoolSize = 2;

		/// <summary>
		/// Whether this pool can extends when call 'spawn' over than initial size limit
		/// </summary>
		public bool CanExtend = true;

		/// <summary>
		/// Amount of items that will increase when resize pool after reach limit
		/// </summary>
		public int ExtendAmount = 2;
	}

	public class PoolManager<T> where T : PoolObject
	{
		/// <summary>
		/// Pool object (blueprint)
		/// </summary>
		[SerializeField]
		private T poolObject;

		/// <summary>
		/// Pool options for this pool
		/// </summary>
		[SerializeField]
		private PoolOptions poolOptions;

		[SerializeField]
		private Transform poolContainer;

		private List<T> poolObjectList;

		/// <summary>
		/// Get item from pool
		/// </summary>
		/// <returns>Pool object from pool</returns>
		public T GetItem()
		{
			int index = poolObjectList.FindIndex(obj => !obj.IsActive);
			if (index == -1)
			{
				int newIndex = poolObjectList.Count;
				if (Resize())
				{
					// index of first item which been added
					index = newIndex;
				}
			}
			return poolObjectList[index];
		}

		/// <summary>
		/// Return item to pool
		/// </summary>
		/// <param name="poolObj">Pool object to be return</param>
		public void ReturnItem(T poolObj)
		{
			if (poolContainer)
			{
				poolObj.transform.SetParent(poolContainer);
			}
			poolObj.OnDespawn();
		}

		private void CreatePool(string name, PoolOptions options)
		{
			int poolSize = poolOptions.InitialPoolSize;
			poolObjectList = new List<T>(poolSize);
			AddObjectToPool(poolSize);
		}

		private void AddObjectToPool(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				T poolObj = poolContainer != null ? GameObject.Instantiate<T>(poolObject, poolContainer) : GameObject.Instantiate<T>(poolObject);
				poolObjectList.Add(poolObj);
			}
		}

		private bool Resize()
		{
			if (!poolOptions.CanExtend || poolOptions.ExtendAmount < 1)
			{
				return false;
			}

			int extendSize = poolOptions.ExtendAmount;
			AddObjectToPool(extendSize);
			return true;
		}
	}
}
