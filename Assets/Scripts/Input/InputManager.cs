using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class InputManager : MonoBehaviour, ISpaceShooterInput
	{
		public event Action<MoveDirection> HorizontalInput;

		public event Action AttackInput;

		public event Action MenuInput;

		public event Action ConfirmInput;

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

		private void Update()
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

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				MenuInput?.Invoke();
			}

			if (Input.GetButtonDown("Submit"))
			{
				ConfirmInput?.Invoke();
			}
		}
	}
}
