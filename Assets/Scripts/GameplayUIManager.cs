using SmileProject.SpaceShooter.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class GameplayUIManager : MonoBehaviour
	{
		[SerializeField]
		private PlayerScoreComp playerScoreComp;

		[SerializeField]
		private PlayerHpComp playerHpComp;

		[SerializeField]
		private WaveChangeComp waveChangComp;

		[SerializeField]
		private GameEndComp gameEndComp;

		[SerializeField]
		private GameplayMenuComp gameplayMenu;

		public void ShowGameClear(int score)
		{
			gameEndComp.Show($"Clear!\nYour score : {score}");
		}

		public void ShowGameOver()
		{
			gameEndComp.Show("Game Over");
		}

		public void SetGameplayMenu(bool isShow)
		{
			if (isShow)
			{
				gameplayMenu.Show();
			}
			else
			{
				gameplayMenu.Hide(true);
			}
		}

		public void ShowWaveChange(int waveNumber, int showTime)
		{
			waveChangComp.ShowWave(waveNumber, showTime);
		}

		public void SetPlayerScore(int score)
		{
			playerScoreComp.SetPlayerScore(score);
		}

		public void SetPlayerHp(int hp)
		{
			playerHpComp.SetPlayerHp(hp);
		}
	}
}