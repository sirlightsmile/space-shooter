using System;
using System.Collections.Generic;
using System.Linq;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	[System.Flags]
	public enum FormationType
	{
		None = 0,  // default value
		LinerOne = 1 << 0,
		LinerTwo = 1 << 1,
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
		private FormationType activeFormation;

		[SerializeField]
		private Transform formationContainer;
		private Dictionary<FormationType, List<FormationPoint>> formationMap = new Dictionary<FormationType, List<FormationPoint>>();

		private void Start()
		{
			Setup();
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
			return flag.IsFlagSet(activeFormation);
		}
	}
}