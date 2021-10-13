

using Newtonsoft.Json;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public static class ResourceLoader
	{
		private const string GameDataPath = "/GameData/gamedata";

		public static GameDataModel LoadGameData()
		{
			string str = Loader.LoadTextFile(GameDataPath);
			GameDataModel gameDataModel = JsonConvert.DeserializeObject<GameDataModel>(str);
			return gameDataModel;
		}
	}
}