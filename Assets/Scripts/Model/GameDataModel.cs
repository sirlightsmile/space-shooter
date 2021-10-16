using System;
using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	[Serializable]
	public class GameDataModel
	{
		[JsonProperty("spaceship_gun_model")]
		public SpaceshipGunModel[] spaceshipGameData { get; private set; }

		[JsonProperty("player_spaceship_model")]
		public SpaceshipModel[] playerSpaceships { get; private set; }

		[JsonProperty("enemy_spaceship_model")]
		public EnemySpaceshipModel[] enemySpaceships { get; private set; }

		[JsonProperty("wave_data_model")]
		public WaveDataModel[] waveData { get; private set; }
	}
}