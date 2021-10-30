using System;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class PlayerHpComp : BaseUIComponent
	{
		[SerializeField]
		private Text playerHpText;

		public void SetPlayerHp(int hp)
		{
			playerHpText.text = $"HP : {hp.ToString()}";
		}
	}
}