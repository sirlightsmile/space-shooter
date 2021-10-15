using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PlayerController
	{
		private PlayerSpaceship player;

		private bool allowControl = true;

		public void SetPlayer(PlayerSpaceship player)
		{
			this.player = player;
			player.Destroyed += OnPlayerDestroyed;
		}

		public void SetAllowControl(bool allowControl)
		{
			this.allowControl = allowControl;
		}

		public void Update()
		{
			//TODO: move to input manager
			if (!allowControl || player == null)
			{
				return;
			}

			MoveDirection direction;
			float axis = Input.GetAxisRaw("Horizontal");
			if (Enum.TryParse<MoveDirection>(axis.ToString(), out direction))
			{
				player.MoveToDirection(direction);
			}

			if (Input.GetButtonDown("Fire1"))
			{
				player.Shoot();
			}
			else if (Input.GetButtonUp("Fire1"))
			{
				//TODO: stop fire
			}
		}

		private void OnPlayerDestroyed(Spaceship spaceship)
		{
			SetAllowControl(false);
		}
	}
}
