using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	public class SpaceShipGunModel
	{
		[JsonProperty("id")]
		/// <summary>
		/// Id of gun
		/// </summary>
		public string ID;

		[JsonProperty("bullet_type")]
		/// <summary>
		/// Type of bullet
		/// </summary>
		public BulletType BulletType;

		[JsonProperty("bullet_asset")]
		/// <summary>
		/// Bullet's asset name
		/// </summary>
		public string BulletAsset;

		[JsonProperty("base_damage")]
		/// <summary>
		/// Base Damage
		/// </summary>
		public int BaseDamage;

		[JsonProperty("damage_increment")]
		/// <summary>
		/// Damage increase per level
		/// </summary>
		public int DamageIncrement;

		[JsonProperty("base_speed")]
		/// <summary>
		/// Base shooting speed
		/// </summary>
		public int BaseSpeed;

		[JsonProperty("speed_increment")]
		/// <summary>
		/// Speed increase per level
		/// </summary>
		public int SpeedIncrement;

		[JsonProperty("max_level")]
		/// <summary>
		/// Max Level
		/// </summary>
		public int MaxLevel;
	}
}