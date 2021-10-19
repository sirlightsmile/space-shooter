namespace SmileProject.Generic
{
	public abstract class SoundKeys : StringEnum<SoundKeys>
	{
		private readonly string assetKey;
		private readonly string mixerKey;
		private readonly bool shouldPreload;
		protected SoundKeys(string value, string assetKey, string mixerKey, bool shouldPreload) : base(value)
		{
			this.assetKey = assetKey;
			this.mixerKey = mixerKey;
			this.shouldPreload = shouldPreload;
		}

		public string GetAssetKey()
		{
			return assetKey;
		}

		public string GetMixerKey()
		{
			return mixerKey;
		}

		public bool ShouldPreload()
		{
			return shouldPreload;
		}
	}
}