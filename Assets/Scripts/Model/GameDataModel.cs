using System;
using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	[Serializable]
	public class GameDataModel
	{
		[JsonProperty("spaceship_gun_model")]
		public SpaceshipGunModel[] SpaceshipGameData { get; private set; }

		[JsonProperty("player_spaceship_model")]
		public SpaceshipModel[] PlayerSpaceships { get; private set; }

		[JsonProperty("enemy_spaceship_model")]
		public EnemySpaceshipModel[] EnemySpaceships { get; private set; }

		[JsonProperty("wave_data_model")]
		public WaveDataModel[] WaveData { get; private set; }
	}
}