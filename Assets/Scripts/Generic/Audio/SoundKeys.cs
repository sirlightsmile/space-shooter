using System.Collections.Generic;

namespace SmileProject.Generic
{
	public abstract class SoundKeys : StringEnum<SoundKeys>
	{
		private static List<SoundKeys> soundList = new List<SoundKeys>();
		private readonly string assetKey;
		private readonly string mixerKey;
		protected SoundKeys(string value, string assetKey, string mixerKey) : base(value)
		{
			this.assetKey = assetKey;
			this.mixerKey = mixerKey;
			soundList.Add(this);
		}

		public string GetAssetKey()
		{
			return assetKey;
		}

		public string GetMixerKey()
		{
			return mixerKey;
		}

		public IEnumerable<SoundKeys> GetAll()
		{
			return soundList;
		}
	}
}