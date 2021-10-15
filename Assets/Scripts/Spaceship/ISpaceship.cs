using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public interface ISpaceship
	{
		void Setup<T>(T spaceshipModel) where T : SpaceshipModel;
		void SetSprite(Sprite sprite);
	}
}