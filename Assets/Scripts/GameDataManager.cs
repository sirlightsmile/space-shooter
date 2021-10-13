using UnityEngine;
using System;

namespace SmileProject.SpaceShooter
{
	public class GameDataManager
	{
		//TODO: load game data by other ways
		public TextAsset gameDataJson;
		public GameDataModel gameData;

		public GameDataManager()
		{
			gameData = JsonUtility.FromJson<GameDataModel>(gameDataJson.text);
		}

		public SpaceShipGunModel GetSpaceShipGunModelById(string id)
		{
			SpaceShipGunModel model = Array.Find(GetSpaceShipGunModels(), (obj) => obj.ID == id);
			return model;
		}

		public SpaceShipGunModel[] GetSpaceShipGunModels()
		{
			Debug.Assert(gameData.spaceShipGameData != null, "SpaceShipGun game data should not be null.");
			return gameData.spaceShipGameData;
		}
	}
}