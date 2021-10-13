using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class GameController : MonoSingleton<GameController>
	{
		public GameDataManager GameDataManager { get; private set; }

		public void Initialize()
		{
			GameDataManager = new GameDataManager();
		}
	}
}