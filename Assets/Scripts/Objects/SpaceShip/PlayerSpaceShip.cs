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
		public int PlayerLevel { get; private set; }
		public PlayerSpaceShip(int hp, float speed, float atk) : base(hp, speed, atk)
		{
			this.PlayerLevel = 1;
		}

		public override void GetHit()
		{
			// player take static damage
			int damage = -1;
			if (AddHP(damage) == 0)
			{
				OnShipDestroy();
			}
		}

		public void MoveToDirection(MoveDirection direction)
		{
			float directionValue = (float)direction;
			float posX = this.transform.position.x + (directionValue * (this.speed * Time.deltaTime));
			this.transform.position = new Vector3(posX, 0, this.transform.position.z);
		}

		private int AddHP(int amount)
		{
			this.hp += amount;
			return this.hp;
		}

		protected override void OnShipDestroy()
		{
			throw new NotImplementedException();
		}
	}
}
