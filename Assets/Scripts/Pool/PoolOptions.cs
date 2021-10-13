namespace SmileProject.SpaceShooter
{
	[System.Serializable]
	public class PoolOptions
	{
		/// <summary>
		/// Pool object prefab
		/// </summary>
		public PoolObject Prefab;

		/// <summary>
		/// Pool name
		/// </summary>
		public string PoolName;

		/// <summary>
		/// Number of initial pool objects in pool when create
		/// </summary>
		public int InitialSize = 2;

		/// <summary>
		/// Whether this pool can extends when call 'spawn' over than initial size limit
		/// </summary>
		public bool CanExtend = true;

		/// <summary>
		/// Amount of items that which will increase when resize pool after reach limit
		/// </summary>
		public int ExtendAmount = 2;
	}
}