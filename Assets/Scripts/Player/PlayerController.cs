using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PlayerController
	{
		[SerializeField]
		private PlayerSpaceShip player;

		[SerializeField]
		private PoolManager<Bullet> playerBulletPool;

		private bool allowControl = true;

		public void Initialize()
		{
			player.Destroyed += OnPlayerDestroyed;
		}

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
				player.MoveToDirection(direction);
			}

			if (Input.GetButtonDown("Fire1"))
			{
				//TODO: fire repeat
			}
			else if (Input.GetButtonUp("Fire1"))
			{
				//TODO: stop fire
			}
		}

		private void OnPlayerDestroyed(System.Object sender, EventArgs args)
		{
			SetAllowControl(false);
		}
	}
}
