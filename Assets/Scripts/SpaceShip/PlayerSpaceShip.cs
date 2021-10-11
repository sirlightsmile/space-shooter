using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public enum MoveDirection
	{
		Left = -1,
		Right = 1
	}

	public class PlayerSpaceShip : SpaceShip
	{
		public EventHandler Destroyed;
		public int PlayerLevel { get; private set; }
		public PlayerSpaceShip(int hp, float speed, float atk) : base(hp, speed, atk)
		{
			this.PlayerLevel = 1;
		}

		public override void GetHit(int damage)
		{
			this.hp -= damage;
		}

		public void MoveToDirection(MoveDirection direction)
		{
			float directionValue = (float)direction;
			float posX = this.transform.position.x + (directionValue * (this.speed * Time.deltaTime));
			this.transform.position = new Vector3(posX, 0, this.transform.position.z);
		}

		protected override void ShipDestroy()
		{
			Destroyed?.Invoke(this, new EventArgs());
		}
	}
}
