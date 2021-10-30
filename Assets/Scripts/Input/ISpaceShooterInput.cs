using System;

namespace SmileProject.SpaceShooter
{
	public interface ISpaceShooterInput
	{
		event Action<MoveDirection> HorizontalInput;

		event Action AttackInput;

		event Action MenuInput;

		void SetAllowInput(bool isAllowInput);

		void SetAllowAttack(bool isAllowAttack);
	}
}
