namespace SmileProject.SpaceShooter
{
	public class SpaceShipGunModel
	{
		/// <summary>
		/// Id of gun
		/// </summary>
		public string ID;

		/// <summary>
		/// Type of bullet
		/// </summary>
		public BulletType BulletType;

		/// <summary>
		/// Bullet's asset name
		/// </summary>
		public string BulletAsset;

		/// <summary>
		/// Base Damage
		/// </summary>
		public int BaseDamage;

		/// <summary>
		/// Damage increase per level
		/// </summary>
		public int DamageIncrement;

		/// <summary>
		/// Base shooting speed
		/// </summary>
		public int BaseSpeed;

		/// <summary>
		/// Speed increase per level
		/// </summary>
		public int SpeedIncrement;

		/// <summary>
		/// Max Level
		/// </summary>
		public int MaxLevel;
	}
}