using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.Generic
{
	public interface IResourceLoader
	{
		Task InitializeAsync();
		Task<T> Load<T>(string key);
		Task<T> LoadJsonAsModel<T>(string key);
		Task<T> InstantiateAsync<T>(object key, Vector3 position) where T : MonoBehaviour;
		void SetSpriteAsync(string key, SpriteRenderer spriteRenderer);
	}
}