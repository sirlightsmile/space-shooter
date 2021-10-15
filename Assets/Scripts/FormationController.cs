using System.Collections.Generic;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	[System.Flags]
	public enum FormationType
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

	public class FormationController : MonoSingleton<FormationController>
	{
		[SerializeField, EnumFlag(EnumFlagAttribute.FlagLayout.List)]
		private FormationType activeFormations;

		[SerializeField]
		private Transform formationContainer;
		private Dictionary<FormationType, List<FormationPoint>> formationMap = new Dictionary<FormationType, List<FormationPoint>>();

		public bool trigger = false;
		public EnemySpaceship enemyPrefab;
		public Transform spawnpoint;

		private void Start()
		{
			Setup();
		}

		private void Update()
		{
			if (!trigger)
			{
				return;
			}

			trigger = false;

			IEnumerable<FormationType> formations = activeFormations.GetFlags<FormationType>();
			foreach (FormationType formation in formations)
			{
				if (formationMap.TryGetValue(formation, out var points))
				{
					foreach (FormationPoint point in points)
					{
						if (point.HasLandedSpaceship())
						{
							continue;
						}
						EnemySpaceship enemy = Instantiate<EnemySpaceship>(enemyPrefab, spawnpoint);
						enemy.MoveToTarget(point.GetPosition());
						point.SetLandedSpaceship(enemy);
					}
				}
			}
		}

		public void Setup()
		{
			FormationPoint[] formationPoints = formationContainer.GetComponentsInChildren<FormationPoint>();
			foreach (FormationPoint point in formationPoints)
			{
				FormationType pointFormation = point.GetFormations();
				IEnumerable<FormationType> flags = pointFormation.GetFlags<FormationType>();
				foreach (FormationType formation in flags)
				{
					if (!formationMap.ContainsKey(formation))
					{
						formationMap.Add(formation, new List<FormationPoint>());
					}
					formationMap[formation].Add(point);
				}
			}
		}

		public bool IsActiveFormation(FormationType flag)
		{
			return flag.IsFlagSet(activeFormations);
		}
	}
}