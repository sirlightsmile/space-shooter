using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class WaveChangeComp : BaseUIComponent
	{
		[SerializeField]
		private Text waveNumberText;

		/// <summary>
		/// Show wave change UI then auto hide
		/// </summary>
		/// <param name="waveNumber">number of wave</param>
		/// <param name="showTime">show time in milliseconds</param>
		/// <returns></returns>
		public void ShowWave(int waveNumber, int showTime)
		{
			waveNumberText.text = $"Wave : {waveNumber}";
			Show(showTime);
		}
	}
}