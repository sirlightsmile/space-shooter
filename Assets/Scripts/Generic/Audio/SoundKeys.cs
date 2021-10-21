using System.Collections.Generic;

namespace SmileProject.Generic
{
	public abstract class SoundKeys : StringEnum<SoundKeys>
	{
		protected static List<SoundKeys> soundList = new List<SoundKeys>();
		protected readonly string assetKey;
		protected readonly string mixerKey;
		protected SoundKeys(string value, string assetKey, string mixerKey) : base(value)
		{
			this.assetKey = assetKey;
			this.mixerKey = mixerKey;
			soundList.Add(this);
		}

		public virtual string GetAssetKey()
		{
			return assetKey;
		}

		public virtual string GetMixerKey()
		{
			return mixerKey;
		}

		public virtual IEnumerable<SoundKeys> GetAll()
		{
			return soundList;
		}
	}
}