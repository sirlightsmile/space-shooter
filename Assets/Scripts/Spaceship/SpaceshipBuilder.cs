
using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class SpaceshipBuilder
	{
		private IResourceLoader resourceLoader;

		public SpaceshipBuilder(IResourceLoader resourceLoader)
		{
			this.resourceLoader = resourceLoader;
		}

		public async Task<T> BuildSpaceship<T>(string templateKey, SpaceshipModel model) where T : MonoBehaviour, ISpaceship
		{
			string spriteName = model.AssetName;
			T spaceship = await resourceLoader.InstantiateAsync<T>(templateKey);
			spaceship.Setup(model);
			resourceLoader.SetSpriteAsync(spriteName, spaceship.SetSprite);
			return spaceship;
		}
	}
}