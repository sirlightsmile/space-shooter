using System.Collections.Generic;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	[System.Flags]
	public enum FormationType
	{
		LinerOne = 1 << 0,
		LinerTwo = 1 << 1,
		LinearThree = 1 << 2,
		LinearFour = 1 << 3,
		SideZigzag = 1 << 4,
		UpperZigZagA = 1 << 5,
		UpperZigZagB = 1 << 6,
		BottomZigzagA = 1 << 7,
		BottomZigzagB = 1 << 8,
		CenterGroup = 1 << 9,
	}

	public class FormationController : MonoSingleton<FormationController>
	{
		private Dictionary<FormationType, bool> formationStatus = new Dictionary<FormationType, bool>();

		public bool IsActiveFormation(FormationType type)
		{
			if (formationStatus.TryGetValue(type, out bool status))
			{
				return status;
			}
			return false;
		}
	}
}