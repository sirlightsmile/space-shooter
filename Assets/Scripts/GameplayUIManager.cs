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
		private GameStartComp gameStartComp;

		[SerializeField]
		private GameOverComp gameOverComp;

		[SerializeField]
		private GameplayMenuComp gameplayMenu;

		public void ShowGameStart()
		{
			//TODO: implement
		}

		public void ShowGameOver()
		{
			//TODO: implement
		}

		public void ShowGameplayMenu()
		{
			//TODO: implement
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