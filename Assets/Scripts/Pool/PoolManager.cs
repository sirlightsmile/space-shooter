using System.Collections.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PoolInfo
	{
		public readonly PoolObject PoolObject;
		public readonly PoolOptions PoolOptions;
		public readonly Transform PoolContainer;
		public readonly List<PoolObject> PoolList;
		public PoolInfo(PoolObject poolObject, PoolOptions options, Transform container)
		{
			this.PoolObject = poolObject;
			this.PoolOptions = options;
			this.PoolContainer = container;
			this.PoolList = new List<PoolObject>(options.InitialPoolSize);
		}
	}

	public class PoolManager : MonoBehaviour
	{
		[SerializeField]
		private Transform poolContainer;
		private Dictionary<string, PoolInfo> poolObjectDict = new Dictionary<string, PoolInfo>();

		/// <summary>
		/// Get item from pool
		/// </summary>
		/// <returns>Pool object from pool</returns>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		/// <returns></returns>
		public T GetItem<T>(string poolName) where T : PoolObject
		{
			PoolInfo poolInfo = GetPoolInfo(poolName);
			if (poolInfo == null)
			{
				return null;
			}

			List<PoolObject> poolObjectList = poolInfo.PoolList;
			int index = poolObjectList.FindIndex(obj => !obj.IsActive);
			if (index == -1)
			{
				int newIndex = poolObjectList.Count;
				if (Resize(poolInfo))
				{
					// index of first item which been added
					index = newIndex;
				}
			}
			return poolObjectList[index] as T;
		}

		/// <summary>
		/// Return item to pool or destroy it if pool not found
		/// </summary>
		/// <param name="poolObj">Pool object to be return</param>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		public void ReturnItem<T>(string poolName, T poolObj) where T : PoolObject
		{
			Transform container = GetPoolInfo(poolName)?.PoolContainer;
			if (container)
			{
				poolObj.transform.SetParent(container);
				poolObj.OnDespawn();
			}
			else
			{
				Destroy(poolObj);
			}
		}

		/// <summary>
		/// Create new pool objects on pool manager
		/// </summary>
		/// <param name="name">Pool name</param>
		/// <param name="poolObject">Pool object prefab</param>
		/// <param name="options">Pool options</param>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		public void CreatePool<T>(string name, T poolObject, PoolOptions options) where T : PoolObject
		{
			if (GetPoolInfo(name) != null)
			{
				Debug.LogAssertion($"Pool name [{name}] already exist.");
				return;
			}

			GameObject container = new GameObject(name);
			container.transform.SetParent(poolContainer);
			PoolInfo poolInfo = new PoolInfo(poolObject, options, container.transform);
			poolObjectDict.Add(name, poolInfo);
			int poolSize = options.InitialPoolSize;
			AddObjectToPool(poolInfo, options.InitialPoolSize);
		}

		private void AddObjectToPool(PoolInfo poolInfo, int extendAmount)
		{
			for (int i = 0; i < extendAmount; i++)
			{
				PoolObject originObj = poolInfo.PoolObject;
				PoolObject poolObj = Instantiate(originObj, poolContainer);
				poolInfo.PoolList.Add(poolObj);
			}
		}

		private bool Resize(PoolInfo poolInfo)
		{
			PoolOptions options = poolInfo.PoolOptions;
			if (!options.CanExtend || options.ExtendAmount < 1)
			{
				return false;
			}

			int extendSize = options.ExtendAmount;
			AddObjectToPool(poolInfo, extendSize);
			return true;
		}

		private PoolInfo GetPoolInfo(string poolName)
		{
			PoolInfo poolInfo = poolObjectDict[poolName];
			if (poolInfo == null)
			{
				Debug.Log($"Pool info name {poolName} not found");
				return null;
			}
			return poolInfo;
		}
	}
}
