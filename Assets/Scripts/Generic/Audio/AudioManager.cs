using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.Audio;

namespace SmileProject.Generic
{
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
		private int playId = 0;

		private void Start()
		{
			audioSources = new List<AudioSource>(audioSourcesContainer.GetComponentsInChildren<AudioSource>());
			playingSource = new Dictionary<int, AudioSource>();
			mixerMap = new Dictionary<string, AudioMixerGroup>();
		}

		/// <summary>
		/// Initialize audio manager
		/// </summary>
		/// <param name="resourceLoader">resource loader</param>
		/// <param name="mixerKey">mixer asset key</param>
		/// <returns></returns>
		public async Task Initialize(IResourceLoader resourceLoader, string mixerKey)
		{
			this.resourceLoader = resourceLoader;
			await InitMixer(mixerKey);
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
	}
}