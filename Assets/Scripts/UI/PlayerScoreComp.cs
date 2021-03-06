using System;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class PlayerScoreComp : BaseUIComponent
	{
		[SerializeField]
		private Text _playerScoreText;

		public void SetPlayerScore(int score)
		{
			_playerScoreText.text = $"Player score : {score}";
		}
	}
}