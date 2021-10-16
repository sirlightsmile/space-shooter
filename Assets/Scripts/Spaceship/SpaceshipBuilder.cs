
using System.Threading.Tasks;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public abstract class SpaceshipBuilder
	{
		private IResourceLoader resourceLoader;

		public SpaceshipBuilder(IResourceLoader resourceLoader)
		{
			this.resourceLoader = resourceLoader;
		}

		public abstract Task<Spaceship> BuildSpaceshipById(string id);

		public async virtual Task<T> BuildSpaceship<T, T2>(string templateKey, T2 model) where T : Spaceship where T2 : SpaceshipModel
		{
			string spriteName = GetAssetPrefix() + model.AssetName;
			T spaceship = await resourceLoader.InstantiateAsync<T>(templateKey);
			spaceship.Setup(model);
			resourceLoader.SetSpriteAsync(spriteName, spaceship.SetSprite);
			return spaceship;
		}

		protected virtual string GetAssetPrefix()
		{
			// default no prefix
			return "";
		}
	}
}