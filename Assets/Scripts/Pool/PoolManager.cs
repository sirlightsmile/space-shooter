using System.Collections.Generic;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PoolInfo
	{
		public readonly PoolOptions Options;
		public readonly Transform Container;
		public readonly List<PoolObject> PoolList;

		public PoolInfo(PoolOptions options, Transform container)
		{
			this.Options = options;
			this.Container = container;
			this.PoolList = new List<PoolObject>(options.InitialSize);
		}
	}

	public class PoolManager : MonoSingleton<PoolManager>
	{
		[SerializeField]
		private Transform poolContainer;
		private Dictionary<string, PoolInfo> poolInfoDict = new Dictionary<string, PoolInfo>();

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
			Transform container = GetPoolInfo(poolName)?.Container;
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
		/// Create new pool on pool manager
		/// </summary>
		/// <param name="options">Pool options</param>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		public void CreatePool(PoolOptions options)
		{
			string name = options.PoolName;
			if (GetPoolInfo(name) != null)
			{
				Debug.LogAssertion($"Pool name [{name}] already exist.");
				return;
			}

			GameObject container = new GameObject(name);
			container.transform.SetParent(poolContainer);
			PoolInfo poolInfo = new PoolInfo(options, container.transform);
			poolInfoDict.Add(name, poolInfo);
			int poolSize = options.InitialSize;
			AddObjectToPool(poolInfo, options.InitialSize);
		}

		/// <summary>
		/// Destroy pool from pool manager
		/// </summary>
		/// <param name="poolName">Pool name to destroy</param>
		public void DestroyPool(string poolName)
		{
			PoolInfo poolInfo = GetPoolInfo(poolName);
			if (poolInfo == null)
			{
				Debug.LogAssertion($"Pool name [{name}] not exist.");
				return;
			}

			List<PoolObject> poolList = poolInfo.PoolList;
			int lastIndex = poolInfo.PoolList.Count - 1;
			for (int i = lastIndex - 1; i >= 0; i--)
			{
				Destroy(poolList[i]);
			}
			poolInfoDict.Remove(poolName);
		}

		/// <summary>
		/// Check whether pool manager contain this pool name or not
		/// /// </summary>
		/// <param name="poolName">target pool name</param>
		/// <returns></returns>
		public bool HasPool(string poolName)
		{
			return poolInfoDict[poolName] != null;
		}

		private void AddObjectToPool(PoolInfo poolInfo, int extendAmount)
		{
			for (int i = 0; i < extendAmount; i++)
			{
				PoolObject prefab = poolInfo.Options.Prefab;
				PoolObject poolObj = Instantiate(prefab, poolContainer);
				poolInfo.PoolList.Add(poolObj);
			}
		}

		private bool Resize(PoolInfo poolInfo)
		{
			PoolOptions options = poolInfo.Options;
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
			PoolInfo poolInfo = poolInfoDict[poolName];
			if (poolInfo == null)
			{
				Debug.Log($"Pool info name {poolName} not found");
				return null;
			}
			return poolInfo;
		}
	}
}
