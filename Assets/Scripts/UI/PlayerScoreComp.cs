using System;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class PlayerScoreComp : BaseUIComponent
	{
		[SerializeField]
		private Text playerScoreText;

		public void SetPlayerScore(int score)
		{
			playerScoreText.text = $"Player score : {score}";
		}
	}
}