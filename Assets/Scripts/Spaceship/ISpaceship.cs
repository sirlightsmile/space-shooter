using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public interface ISpaceship
	{
		void Setup(SpaceshipModel model);
		void SetSprite(Sprite sprite);
	}
}