using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public enum MoveDirection
	{
		Left = -1,
		Right = 1
	}

	public class PlayerSpaceship : Spaceship
	{
		public int PlayerLevel { get; private set; } = playerInitialLevel;

		private const int playerInitialLevel = 1;
		private float moveBorder = 0;

		private void Start()
		{
			SetBorder();
		}

		public override void GetHit(int damage)
		{
			base.GetHit(damage);
			//TODO: invoke dead
		}

		public void MoveToDirection(MoveDirection direction)
		{
			float directionValue = (float)direction;
			float posX = this.transform.position.x + (directionValue * (this.speed * Time.deltaTime));
			posX = Mathf.Clamp(posX, -moveBorder, moveBorder);
			this.transform.position = new Vector3(posX, this.transform.position.y, this.transform.position.z);
		}

		/// <summary>
		/// Setup world border for move
		/// </summary>
		private void SetBorder()
		{
			float halfSize = this.width / 2;
			float borderRight = Screen.width - halfSize;
			float borderWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(borderRight, 0, 0)).x;
			this.moveBorder = borderWorldPoint;
		}
	}
}
