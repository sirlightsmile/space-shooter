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

		[JsonProperty("wave_spawns_data")]

		/// <summary>
		/// Array of formation and spaceship spawn
		/// </summary>
		public WaveSpawnData[] WaveSpawnsData { get; private set; }

		[JsonProperty("spawn_interval")]
		/// <summary>
		/// Time interval for spawns each WaveSpawnData (second)
		/// </summary>
		public float SpawnInterval { get; private set; }
	}

	public class WaveSpawnData
	{
		[JsonProperty("formation")]
		/// <summary>
		/// Spawn formation
		/// </summary>
		public Formation Formation { get; private set; }

		[JsonProperty("spawn_spaceship_id")]
		/// <summary>
		/// Spaceship id that will be spawn on this formation
		/// </summary>
		public string SpawnSpaceshipID { get; private set; }
	}
}