using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace SmileProject.Generic
{
	/// <summary>
	/// Addressable resource loader manager
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject audioSourcesContainer;

		[SerializeField]
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
		/// <param name="mainMixerKey">mixer asset key</param>
		/// <returns></returns>
		public async Task Initialize(IResourceLoader resourceLoader, string mainMixerKey)
		{
			this.resourceLoader = resourceLoader;
			await InitMixer(mainMixerKey);
		}

		/// <summary>
		/// Preload sound from sound key
		/// </summary>
		/// <param name="soundKeys"></param>
		/// <returns></returns>
		public async Task PreloadSounds(SoundKeys[] soundKeys)
		{
			List<string> preloadList = new List<string>();
			foreach (SoundKeys soundKey in soundKeys)
			{
				preloadList.Add(soundKey.GetAssetKey());
			}
			await resourceLoader.Preload(preloadList);
		}

		/// <summary>
		/// Load clip then play sound through selected mixer if that mixer is exist when initialize
		/// </summary>
		/// <param name="soundKey">asset key of audio clip</param>
		/// <param name="loop">is loop sound</param>
		/// <param name="mixer">selected mixer</param>
		/// <returns>play id</returns>
		public async Task<int> PlaySound(SoundKeys soundKey, bool loop = false)
		{
			AudioClip clip = await resourceLoader.Load<AudioClip>(soundKey.GetAssetKey());
			AudioSource source = GetAvaliableAudioSource();
			string mixerKey = soundKey.GetMixerKey();
			if (mixerKey != null && mixerMap.TryGetValue(mixerKey, out AudioMixerGroup mixerGroup))
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
			Debug.Log($"Play sound : {soundKey.ToString()}");
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
				playingSource.Remove(playId);
			}
		}

		private AudioSource GetAvaliableAudioSource()
		{
			AudioSource audioSource = audioSources.Find(item => !item.isPlaying);
			if (audioSource == null)
			{
				audioSource = audioSourcesContainer.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
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