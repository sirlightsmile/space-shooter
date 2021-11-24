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
		public override SpaceshipTag SpaceshipTag { get { return SpaceshipTag.Player; } }
		public int PlayerLevel { get; private set; } = PLAYER_INITIAL_LEVEL;

		private const int PLAYER_INITIAL_LEVEL = 1;
		private float _moveBorder = 0;

		/// <summary>
		/// Move spaceship in horizontal direction
		/// </summary>
		/// <param name="direction">move direction</param>
		public void MoveToDirection(MoveDirection direction)
		{
			float directionValue = (float)direction;
			float posX = this.transform.position.x + (directionValue * (_speed * Time.deltaTime));
			posX = Mathf.Clamp(posX, -_moveBorder, _moveBorder);
			this.transform.position = new Vector3(posX, this.transform.position.y, this.transform.position.z);
		}

		/// <summary>
		/// Set sprite image
		/// </summary>
		/// <param name="sprite">sprite image</param>
		public override void SetSprite(Sprite sprite)
		{
			base.SetSprite(sprite);
			SetBorder();
		}

		/// <summary>
		/// Setup world border for move
		/// </summary>
		private void SetBorder()
		{
			float halfSize = _width / 2;
			float borderRight = Screen.width - halfSize;
			float borderWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(borderRight, 0, 0)).x;
			_moveBorder = borderWorldPoint;
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			this.gameObject.SetActive(false);
		}
	}
}
