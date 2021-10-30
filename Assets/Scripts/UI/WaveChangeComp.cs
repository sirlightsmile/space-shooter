using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class WaveChangeComp : MonoBehaviour
	{
		[SerializeField]
		private Text waveNumberText;

		[SerializeField]
		private Animator animator;

		/// <summary>
		/// Show wave change UI then auto hide
		/// </summary>
		/// <param name="waveNumber">number of wave</param>
		/// <param name="showTime">show time in milliseconds</param>
		/// <returns></returns>
		public async void Show(int waveNumber, int showTime)
		{
			this.gameObject.SetActive(true);
			waveNumberText.text = $"Wave : {waveNumber}";

			await Task.Delay(showTime);
			Hide();
		}

		private void Hide()
		{
			animator.SetTrigger("Hide");
		}

		/// <summary>
		/// Trigger by hide animation
		/// </summary>
		private void OnHide()
		{
			this.gameObject.SetActive(false);
		}
	}
}