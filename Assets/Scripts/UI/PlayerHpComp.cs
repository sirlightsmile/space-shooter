using System;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class PlayerHpComp : BaseUIComponent
	{
		[SerializeField]
		private Text _playerHpText;

		public void SetPlayerHp(int hp)
		{
			_playerHpText.text = $"HP : {hp.ToString()}";
		}
	}
}