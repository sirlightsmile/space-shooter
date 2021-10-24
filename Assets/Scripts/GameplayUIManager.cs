using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter
{
	public class GameplayUIManager : MonoBehaviour
	{
		[SerializeField]
		private Text playerScoreText;

		[SerializeField]
		private Text playerHpText;

		public void SetPlayerScore(int score)
		{
			playerScoreText.text = $"Player score : {score}";
		}

		public void SetPlayerHp(int hp)
		{
			playerHpText.text = $"HP : {hp.ToString()}";
		}
	}
}