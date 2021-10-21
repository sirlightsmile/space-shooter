using System.IO;
using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class MixerGroup : StringEnum<MixerGroup>
	{
		public MixerGroup(string value) : base(value)
		{
		}

		public static readonly MixerGroup BGM = new MixerGroup("BGM");
		public static readonly MixerGroup SoundEffect = new MixerGroup("SoundEffect");
	}

	public sealed class GameSoundKeys : SoundKeys
	{
		private const string assetPath = "GameplaySounds/";
		public GameSoundKeys(string value, string assetKey, string mixerKey) : base(value, assetKey, mixerKey)
		{
		}

		public override string GetAssetKey()
		{
			string assetKey = base.GetAssetKey();
			string fullPath = Path.Combine(assetPath, assetKey);
			return fullPath;
		}

		#region BGM
		public static readonly GameSoundKeys GameplayBGM = new GameSoundKeys(nameof(GameplayBGM), "GameplayBGM.mp3", MixerGroup.BGM.ToString());
		#endregion

		#region Sound effects
		public static readonly GameSoundKeys DiveBomb = new GameSoundKeys(nameof(DiveBomb), "DiveBomb.wav", MixerGroup.SoundEffect.ToString());
		public static readonly GameSoundKeys Explosion = new GameSoundKeys(nameof(Explosion), "Explosion.wav", MixerGroup.SoundEffect.ToString());
		public static readonly GameSoundKeys Failed = new GameSoundKeys(nameof(Failed), "Failed.mp3", MixerGroup.SoundEffect.ToString());
		public static readonly GameSoundKeys Shoot = new GameSoundKeys(nameof(Shoot), "LaserShoot.wav", MixerGroup.SoundEffect.ToString());
		public static readonly GameSoundKeys PlayerExplosion = new GameSoundKeys(nameof(PlayerExplosion), "PlayerExplosion.wav", MixerGroup.SoundEffect.ToString());
		public static readonly GameSoundKeys PowerUp = new GameSoundKeys(nameof(PowerUp), "PowerUp.mp3", MixerGroup.SoundEffect.ToString());
		public static readonly GameSoundKeys Succeed = new GameSoundKeys(nameof(Succeed), "Succeed.mp3", MixerGroup.SoundEffect.ToString());
		#endregion
	}
}
