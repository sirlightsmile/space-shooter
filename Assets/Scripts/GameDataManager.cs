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

		public SpaceshipGunModel GetSpaceshipGunModelById(string id)
		{
			SpaceshipGunModel model = Array.Find(GetSpaceshipGunModels(), (obj) => obj.ID == id);
			Debug.Assert(model != null, $"SpaceShipGun game data id : [{id}] not found.");
			return model;
		}

		public SpaceshipGunModel[] GetSpaceshipGunModels()
		{
			Debug.Assert(gameData.spaceshipGameData != null, "SpaceShipGun game data should not be null.");
			return gameData.spaceshipGameData;
		}
	}
}