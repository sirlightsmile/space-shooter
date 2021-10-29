using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public interface ISpaceShooterInput
	{
		event Action<MoveDirection> HorizontalInput;

		event Action AttackInput;

		void SetAllowInput(bool isAllowInput);

		void SetAllowAttack(bool isAllowAttack);

		void Update();
	}
}
