
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

		public async virtual Task<T> BuildSpaceship<T, T2>(string templateKey, T2 model) where T : MonoBehaviour, ISpaceship where T2 : SpaceshipModel
		{
			string spriteName = model.AssetName;
			T spaceship = await resourceLoader.InstantiateAsync<T>(templateKey);
			spaceship.Setup(model);
			resourceLoader.SetSpriteAsync(spriteName, spaceship.SetSprite);
			return spaceship;
		}
	}
}