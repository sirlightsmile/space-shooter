using Newtonsoft.Json;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceshipModel : SpaceshipModel
	{
		[JsonProperty("score")]
		/// <summary>
		/// Score player will get when destroyed
		/// </summary>
		public int Score { get; private set; }
	}
}