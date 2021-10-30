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
		private GameStartComp gameStartComp;

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

		public void ShowWaveChange(int waveNumber, int showTime)
		{
			waveChangComp.ShowWave(waveNumber, showTime);
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

		public void SetShowPlayerInfo(bool isShow)
		{
			if (isShow)
			{
				playerScoreComp.Show();
				playerHpComp.Show();
			}
			else
			{
				playerScoreComp.Hide();
				playerHpComp.Hide();
			}
		}

		public void SetShowGameStart(bool isShow)
		{
			if (isShow)
			{
				gameStartComp.Show();
			}
			else
			{
				gameStartComp.Hide();
			}
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