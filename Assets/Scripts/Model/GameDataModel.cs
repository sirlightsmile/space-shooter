using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	public class GameDataModel
	{
		[JsonProperty("spaceship_gun_model")]
		public SpaceshipGunModel[] spaceshipGameData { get; private set; }
	}
}