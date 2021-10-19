using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System;

namespace SmileProject.Generic
{
	[Serializable]
	public class AudioInfo
	{
		public string Key;

		public AssetReference AudioReferences;
	}

	/// <summary>
	/// Addressable resource loader manager
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		[SerializeField]
		private List<AudioInfo> audioReferences = new List<AudioInfo>();
		private GameObject audioSourcesContainer;
		private List<AudioSource> audioSources;
		private Dictionary<int, AudioSource> playingSource;
		private IResourceLoader resourceLoader;
		private int playId = 0;


		private void Start()
		{
			audioSources = new List<AudioSource>(audioSourcesContainer.GetComponentsInChildren<AudioSource>());
			playingSource = new Dictionary<int, AudioSource>();
		}

		public async void Initialize(IResourceLoader resourceLoader)
		{
			this.resourceLoader = resourceLoader;
			// await audioReferences[0]
		}

		public async Task<int> PlaySound(string soundKey, bool loop = false)
		{
			AudioClip clip = await resourceLoader.Load<AudioClip>(soundKey);
			AudioSource source = GetAvaliableAudioSource();
			source.clip = clip;
			source.loop = loop;
			source.Play();
			playingSource.Add(playId, source);
			return playId++;
		}

		public void StopSound(int playId)
		{
			if (playingSource.TryGetValue(playId, out AudioSource source))
			{
				if (source.isPlaying)
				{
					source.Stop();
				}
			}
		}

		private AudioSource GetAvaliableAudioSource()
		{
			AudioSource audioSource = audioSources.Find(item => !item.isPlaying);
			if (audioSource == null)
			{
				audioSource = audioSourcesContainer.AddComponent<AudioSource>();
				audioSources.Add(audioSource);
			}
			return audioSource;
		}
	}
}