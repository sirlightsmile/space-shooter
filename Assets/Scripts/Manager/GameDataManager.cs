using UnityEngine;
using System;
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class GameDataManager
	{
		private const string GAME_DATA_KEY = "GameData";
		private GameDataModel _gameData;

		/// <summary>
		/// Load game data to game data manager. Call only once per session.
		/// </summary>
		/// <returns></returns>
		public async Task Initialize(IResourceLoader resourceLoader)
		{
			_gameData = await resourceLoader.LoadJsonAsModel<GameDataModel>(GAME_DATA_KEY);
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
			Debug.Assert(_gameData.SpaceshipGameData != null, "SpaceShipGun game data should not be null.");
			return _gameData.SpaceshipGameData;
		}

		public WaveDataModel[] GetWaveDataModels()
		{
			Debug.Assert(_gameData.WaveData != null, "Wave game data should not be null.");
			return _gameData.WaveData;
		}

		public WaveDataModel GetWaveDataModelByWaveNumber(int number)
		{
			WaveDataModel model = Array.Find(GetWaveDataModels(), (obj) => obj.WaveNumber == number);
			Debug.Assert(model != null, $"WaveDataModel game data number : [{number}] not found.");
			return model;
		}

		public EnemySpaceshipModel[] GetEnemySpaceshipModels()
		{
			Debug.Assert(_gameData.EnemySpaceships != null, "Enemy spaceship game data should not be null.");
			return _gameData.EnemySpaceships;
		}

		public EnemySpaceshipModel GetEnemySpaceshipModelById(string id)
		{
			EnemySpaceshipModel model = Array.Find(GetEnemySpaceshipModels(), (obj) => obj.ID == id);
			Debug.Assert(model != null, $"EnemySpaceshipModel game data number : [{id}] not found.");
			return model;
		}

		public SpaceshipModel[] GetPlayerSpaceshipModels()
		{
			Debug.Assert(_gameData.PlayerSpaceships != null, "Enemy spaceship game data should not be null.");
			return _gameData.PlayerSpaceships;
		}

		public SpaceshipModel GetPlayerSpaceshipModelById(string id)
		{
			SpaceshipModel model = Array.Find(GetPlayerSpaceshipModels(), (obj) => obj.ID == id);
			Debug.Assert(model != null, $"PlayerSpaceshipModel game data number : [{id}] not found.");
			return model;
		}
	}
}