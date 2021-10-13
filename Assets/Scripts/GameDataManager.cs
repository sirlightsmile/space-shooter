using UnityEngine;
using System;
using System.Threading.Tasks;

namespace SmileProject.SpaceShooter
{
	public class GameDataManager
	{
		private GameDataModel gameData;

		public async Task Initialize()
		{
			gameData = await ResourceLoader.LoadGameData();
			Debug.Log("Game Data Initialized.");
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