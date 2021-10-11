using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private PlayerSpaceShip player;

		[SerializeField]
		private PoolManager<Bullet> playerBulletPool;


		public void Update()
		{
			MoveDirection direction;
			float axis = Input.GetAxisRaw("Horizontal");
			if (Enum.TryParse<MoveDirection>(axis.ToString(), out direction))
			{
				player.MoveToDirection(direction);
			}
		}
	}
}
