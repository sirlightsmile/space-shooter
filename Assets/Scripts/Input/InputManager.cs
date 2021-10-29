using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class InputManager : ISpaceShooterInput
	{
		public event Action<MoveDirection> HorizontalInput;

		public event Action AttackInput;

		private bool allowInput = true;
		private bool allowAttack = true;

		/// <summary>
		/// Set permission for every input.
		/// </summary>
		/// <param name="allowInput">whether allow input or not</param>
		public void SetAllowInput(bool allowInput)
		{
			this.allowInput = allowInput;
		}

		/// <summary>
		/// Set permission for attack input
		/// </summary>
		/// <param name="allowAttack">whether allow attack input or not</param>
		public void SetAllowAttack(bool allowAttack)
		{
			this.allowAttack = allowAttack;
		}

		public void Update()
		{
			if (!allowInput)
			{
				return;
			}

			MoveDirection direction;
			float axis = Input.GetAxisRaw("Horizontal");
			if (Enum.TryParse<MoveDirection>(axis.ToString(), out direction))
			{
				HorizontalInput?.Invoke(direction);
			}

			if (allowAttack && Input.GetButtonDown("Fire1"))
			{
				AttackInput?.Invoke();
			}
		}
	}
}
