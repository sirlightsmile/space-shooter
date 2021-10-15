using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	public class WaveDataModel
	{
		[JsonProperty("wave_number")]
		/// <summary>
		/// Number of wave
		/// </summary>
		public int WaveNumber { get; private set; }

		[JsonProperty("enemy_spawn_data")]
		/// <summary>
		/// Spaceship sprite asset id from addressable assets
		/// </summary>
		public string EnemySpawnData { get; private set; }

		[JsonProperty("spawn_interval")]
		/// <summary>
		/// Time interval between each enemy spawn
		/// </summary>
		public float SpawnInterval { get; private set; }
	}

	public class EnemySpawnData
	{
		[JsonProperty("formation")]
		/// <summary>
		/// Spawn formation
		/// </summary>
		public FormationType Formation { get; private set; }

		[JsonProperty("spawn_enemy_id")]
		/// <summary>
		/// Enemy spawn to this formation
		/// </summary>
		public string SpawnEnemyID { get; private set; }
	}
}