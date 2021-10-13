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
			PoolInfo poolInfo = GetPoolInfo(poolName);
			Transform container = poolInfo?.Container;
			if (poolInfo != null)
			{
				poolObj.SetParent(container);
				poolObj.OnDespawn();
			}
			else
			{
				// pool already destroyed
				Destroy(poolObj.gameObject);
			}
		}

		/// <summary>
		/// Create new pool on pool manager
		/// </summary>
		/// <param name="options">Pool options</param>
		/// <typeparam name="T">T inherited PoolObject</typeparam>
		public void CreatePool(PoolOptions options)
		{
			string poolName = options.PoolName;
			if (GetPoolInfo(poolName) != null)
			{
				Debug.LogAssertion($"Pool name [{poolName}] already exist.");
				return;
			}

			GameObject container = new GameObject(poolName);
			container.transform.SetParent(poolContainer);
			PoolInfo poolInfo = new PoolInfo(options, container.transform);
			poolInfoDict.Add(poolName, poolInfo);
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
				Debug.LogAssertion($"Pool name [{poolName}] not exist.");
				return;
			}
			Destroy(poolInfo.Container.gameObject);
			poolInfoDict.Remove(poolName);
			Debug.Log($"Destroy pool : [{poolName}]");
		}

		/// <summary>
		/// Check whether pool manager contain this pool name or not
		/// /// </summary>
		/// <param name="poolName">target pool name</param>
		/// <returns></returns>
		public bool HasPool(string poolName)
		{
			return poolInfoDict.ContainsKey(poolName);
		}

		private void AddObjectToPool(PoolInfo poolInfo, int extendAmount)
		{
			for (int i = 0; i < extendAmount; i++)
			{
				PoolOptions options = poolInfo.Options;
				PoolObject prefab = options.Prefab;
				PoolObject poolObj = Instantiate(prefab, poolContainer);

				Transform container = poolInfo.Container;
				if (container)
				{
					poolObj.SetParent(container);
				}
				poolObj.SetPoolName(options.PoolName);
				poolObj.SetActive(false);
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
			PoolInfo poolInfo;
			poolInfoDict.TryGetValue(poolName, out poolInfo);
			if (poolInfo == null)
			{
				Debug.Log($"Pool info name {poolName} not found");
				return null;
			}
			return poolInfo;
		}
	}
}
