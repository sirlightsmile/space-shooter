using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	[System.Flags]
	[JsonConverter(typeof(FlagConverter))]
	public enum Formation
	{
		LinearOne = 1 << 0,
		LinearTwo = 1 << 1,
		LinearThree = 1 << 2,
		LinearFour = 1 << 3,
		SideZigzag = 1 << 4,
		UpperZigZag = 1 << 5,
		BottomZigzag = 1 << 6,
		CenterGroup = 1 << 7,
	}

	public delegate void WaveChangeEventHandler(int newWave);

	public class FormationController : MonoBehaviour
	{
		/// <summary>
		/// Invoke when add new spaceship to formation
		/// </summary>
		public event Action<Spaceship> SpaceshipAdded;

		/// <summary>
		/// Invoke when formation is changeing
		/// </summary>
		public event Action FormationChange;

		/// <summary>
		/// Invoke when all spaceship reached all formation point
		/// </summary>
		public event Action FormationReady;

		private const string moveAnimatorParameter = "isMove";

		[SerializeField, EnumFlag(EnumFlagAttribute.FlagLayout.List)]
		private Formation activeFormations;

		[SerializeField]
		private Transform formationContainer;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private Vector3 spawnPoint = Vector3.zero;

		private Dictionary<Formation, List<FormationPoint>> formationMap = new Dictionary<Formation, List<FormationPoint>>();
		private GameDataManager gameDataManager;
		private SpaceshipBuilder spaceshipBuilder;

		private void Start()
		{
			SetupFormationMap();
		}

		public void Initialize(GameDataManager gameDataManager, SpaceshipBuilder spaceshipBuilder)
		{
			this.gameDataManager = gameDataManager;
			this.spaceshipBuilder = spaceshipBuilder;
			spaceshipBuilder.SpaceshipBuilded += OnSpaceshipBuilded;
		}

		public void OnWaveChanged(int waveNumber)
		{
			Debug.Log("On wave changed");

			// clear all active flags
			activeFormations.ClearFlags<Formation>(activeFormations);
			UpdateActiveFormation(waveNumber);
		}

		private void UpdateActiveFormation(int waveNumber)
		{
			FormationChange?.Invoke();
			Debug.Log("Invoke Formation Change");
			SetMoveAnimation(false);
			WaveDataModel waveData = gameDataManager.GetWaveDataModelByWaveNumber(waveNumber);
			GenerateSpaceshipFromWaveData(waveData);
		}

		private async void GenerateSpaceshipFromWaveData(WaveDataModel waveData)
		{
			WaveSpawnData[] spawnsData = waveData.WaveSpawnsData;
			float interval = waveData.SpawnInterval;
			foreach (WaveSpawnData data in spawnsData)
			{
				string spaceshipId = data.SpawnSpaceshipID;
				Formation formationData = data.Formation;
				IEnumerable<Formation> formations = formationData.GetFlags<Formation>();
				List<Task> spawnTasks = new List<Task>();
				foreach (Formation formation in formations)
				{
					if (formationMap.TryGetValue(formation, out var points))
					{
						foreach (FormationPoint point in points)
						{
							if (point.HasLandedSpaceship())
							{
								continue;
							}
							spawnTasks.Add(SendSpaceshipToPoint(spaceshipId, point));
						}
					}
				}
				await Task.WhenAll(spawnTasks);
				if (interval > 0)
				{
					// convert second to millisecond
					int waitTime = (int)(interval * 1000);
					await Task.Delay(waitTime);
				}
			}

			FormationReady?.Invoke();
			SetMoveAnimation(true);
			Debug.Log("Invoke Formation Ready");
		}

		private void SetMoveAnimation(bool isMove)
		{
			animator.SetBool(moveAnimatorParameter, isMove);
		}

		private async Task SendSpaceshipToPoint(string spaceshipId, FormationPoint point)
		{
			Spaceship spaceship = await this.spaceshipBuilder.BuildSpaceshipById(spaceshipId);
			bool isArrived = false;
			spaceship.MoveToTarget(point.transform, () => { isArrived = true; });
			spaceship.SetPosition(spawnPoint);
			point.SetLandedSpaceship(spaceship);
			await TaskExtensions.WaitUntil(() => isArrived);
		}

		public void SetupFormationMap()
		{
			FormationPoint[] formationPoints = formationContainer.GetComponentsInChildren<FormationPoint>();
			foreach (FormationPoint point in formationPoints)
			{
				Formation pointFormation = point.GetFormations();
				IEnumerable<Formation> flags = pointFormation.GetFlags<Formation>();
				foreach (Formation formation in flags)
				{
					if (!formationMap.ContainsKey(formation))
					{
						formationMap.Add(formation, new List<FormationPoint>());
					}
					formationMap[formation].Add(point);
				}
			}
		}

		public bool IsActiveFormation(Formation flag)
		{
			return flag.IsFlagSet(activeFormations);
		}

		private void OnSpaceshipBuilded(Spaceship spaceship)
		{
			SpaceshipAdded?.Invoke(spaceship);
		}
	}
}