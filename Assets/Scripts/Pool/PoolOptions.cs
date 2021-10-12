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
}