using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;

namespace SmileProject.Generic
{
	[Serializable]
	public class AudioInfo
	{
		public string PlayKey;
		public string AssetKey;
		public string MixerKey;
		public bool ShouldPreload;
	}

	/// <summary>
	/// Addressable resource loader manager
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		private GameObject audioSourcesContainer;
		private List<AudioSource> audioSources;
		private IResourceLoader resourceLoader;
		private Dictionary<int, AudioSource> playingSource;
		private Dictionary<string, AudioMixerGroup> mixerMap;
		private Dictionary<string, AudioInfo> audioInfoMap;
		private int playId = 0;

		private void Start()
		{
			audioSources = new List<AudioSource>(audioSourcesContainer.GetComponentsInChildren<AudioSource>());
			playingSource = new Dictionary<int, AudioSource>();
			mixerMap = new Dictionary<string, AudioMixerGroup>();
			audioInfoMap = new Dictionary<string, AudioInfo>();
		}

		/// <summary>
		/// Initialize audio manager
		/// </summary>
		/// <param name="resourceLoader">resource loader</param>
		/// <param name="mixerKey">mixer asset key</param>
		/// <returns></returns>
		public async Task Initialize(IResourceLoader resourceLoader, string mixerKey, AudioInfo[] audioInfos)
		{
			this.resourceLoader = resourceLoader;
			await Task.WhenAll(new Task[] { InitMixer(mixerKey), SetupAudioInfos(audioInfos) });
		}

		/// <summary>
		/// Load clip then play sound through selected mixer if that mixer is exist when initialize
		/// </summary>
		/// <param name="audioClipKey">asset key of audio clip</param>
		/// <param name="loop">is loop sound</param>
		/// <param name="mixer">selected mixer</param>
		/// <returns>play id</returns>
		public async Task<int> PlaySound(string audioClipKey, bool loop = false, string mixer = null)
		{
			AudioClip clip = await resourceLoader.Load<AudioClip>(audioClipKey);
			AudioSource source = GetAvaliableAudioSource();
			if (mixer != null && mixerMap.TryGetValue(mixer, out AudioMixerGroup mixerGroup))
			{
				source.outputAudioMixerGroup = mixerGroup;
			}
			else
			{
				source.outputAudioMixerGroup = null;
			}
			source.clip = clip;
			source.loop = loop;
			source.Play();
			playingSource.Add(playId, source);
			return playId++;
		}

		/// <summary>
		/// Stop sound that already play
		/// </summary>
		/// <param name="playId">play id</param>
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

		private async Task InitMixer(string mixerKey)
		{
			if (!string.IsNullOrEmpty(mixerKey))
			{
				AudioMixer mixer = await resourceLoader.Load<AudioMixer>(mixerKey);
				if (mixer != null)
				{
					// find all mixer group
					AudioMixerGroup[] groups = mixer.FindMatchingGroups(string.Empty);
					foreach (var group in groups)
					{
						mixerMap.Add(group.name, group);
					}
				}
			}
		}

		private async Task SetupAudioInfos(AudioInfo[] infoList)
		{
			List<string> preloadList = new List<string>();
			foreach (AudioInfo info in infoList)
			{
				string key = info.PlayKey;
				if (audioInfoMap.ContainsKey(key))
				{
					Debug.LogAssertion($"Duplicated audio key [{key}]");
					continue;
				}
				bool shouldPreload = info.ShouldPreload;

				if (shouldPreload)
				{
					string assetKey = info.AssetKey;
					preloadList.Add(assetKey);
				}

				audioInfoMap.Add(key, info);
			}
			await resourceLoader.Preload(preloadList);
		}
	}
}