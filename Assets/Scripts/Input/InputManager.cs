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

		private bool _allowInput = true;
		private bool _allowAttack = true;

		private float _invokeAttackInterval = 0.3f;
		private float _attackTimestamp = 0;

		/// <summary>
		/// Set permission for every input.
		/// </summary>
		/// <param name="allowInput">whether allow input or not</param>
		public void SetAllowInput(bool allowInput)
		{
			_allowInput = allowInput;
		}

		/// <summary>
		/// Set permission for attack input
		/// </summary>
		/// <param name="allowAttack">whether allow attack input or not</param>
		public void SetAllowAttack(bool allowAttack)
		{
			_allowAttack = allowAttack;
		}

		private void Update()
		{
			if (!_allowInput)
			{
				return;
			}

			MoveDirection direction;
			float axis = Input.GetAxisRaw("Horizontal");
			if (Enum.TryParse<MoveDirection>(axis.ToString(), out direction))
			{
				HorizontalInput?.Invoke(direction);
			}

			if (_allowAttack)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					InvokeAttackInput();
				}
				else if (Input.GetButton("Fire1"))
				{
					InvokeAttackInterval();
				}
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

		private void InvokeAttackInterval()
		{
			if (Time.time - _attackTimestamp > _invokeAttackInterval)
			{
				InvokeAttackInput();
			}
		}

		private void InvokeAttackInput()
		{
			_attackTimestamp = Time.time;
			AttackInput?.Invoke();
		}
	}
}
