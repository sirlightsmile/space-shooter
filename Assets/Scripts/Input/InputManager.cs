using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class InputManager : ISpaceShooterInput
	{
		public event Action<MoveDirection> HorizontalInput;

		public event Action AttackInput;

		private bool allowControl = true;

		public void SetAllowControl(bool allowControl)
		{
			this.allowControl = allowControl;
		}

		public void Update()
		{
			if (!allowControl)
			{
				return;
			}

			MoveDirection direction;
			float axis = Input.GetAxisRaw("Horizontal");
			if (Enum.TryParse<MoveDirection>(axis.ToString(), out direction))
			{
				HorizontalInput?.Invoke(direction);
			}

			if (Input.GetButtonDown("Fire1"))
			{
				AttackInput?.Invoke();
			}
		}
	}
}
