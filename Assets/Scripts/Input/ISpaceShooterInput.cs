using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public interface ISpaceShooterInput
	{
	    event Action<MoveDirection> HorizontalInput;

	    event Action AttackInput;

		void SetAllowControl(bool allowControl);

		void Update();
	}
}
