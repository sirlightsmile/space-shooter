using UnityEngine;
using System;

namespace SmileProject.SpaceShooter
{
	public class GameDataManager
	{
		private GameDataModel gameData;

		public GameDataManager()
		{
			gameData = ResourceLoader.LoadGameData(); ;
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