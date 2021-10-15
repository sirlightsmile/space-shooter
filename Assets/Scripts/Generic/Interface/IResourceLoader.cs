using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic
{
	public interface IResourceLoader
	{
		Task InitializeAsync();
		Task<T> Load<T>(string key);
		Task<T> LoadJsonAsModel<T>(string key);
		void SetSpriteAsync(string key, SpriteRenderer spriteRenderer);
	}
}