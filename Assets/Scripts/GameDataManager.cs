using UnityEngine;
using System;
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class GameDataManager
	{
		private const string gameDataKey = "GameData";
		private GameDataModel gameData;

		/// <summary>
		/// Load game data to game data manager. Call only once per session.
		/// </summary>
		/// <returns></returns>
		public async Task Initialize(IResourceLoader resourceLoader)
		{
			gameData = await resourceLoader.LoadJsonAsModel<GameDataModel>(gameDataKey);
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

		public WaveDataModel[] GetWaveDataModels()
		{
			Debug.Assert(gameData.waveData != null, "Wave game data should not be null.");
			return gameData.waveData;
		}

		public WaveDataModel GetWaveDataModelByWaveNumber(int number)
		{
			WaveDataModel model = Array.Find(GetWaveDataModels(), (obj) => obj.WaveNumber == number);
			Debug.Assert(model != null, $"WaveDataModel game data number : [{number}] not found.");
			return model;
		}

		public EnemySpaceshipModel[] GetEnemySpaceshipModels()
		{
			Debug.Assert(gameData.enemySpaceships != null, "Enemy spaceship game data should not be null.");
			return gameData.enemySpaceships;
		}

		public EnemySpaceshipModel GetEnemySpaceshipModelById(string id)
		{
			EnemySpaceshipModel model = Array.Find(GetEnemySpaceshipModels(), (obj) => obj.ID == id);
			Debug.Assert(model != null, $"EnemySpaceshipModel game data number : [{id}] not found.");
			return model;
		}
	}
}