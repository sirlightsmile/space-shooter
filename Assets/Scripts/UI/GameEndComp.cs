using System;
using UnityEngine;
using UnityEngine.UI;

namespace SmileProject.SpaceShooter.UI
{
	public class GameEndComp : BaseUIComponent
	{
		[SerializeField]
		private Text endText;

		/// <summary>
		/// Show end game ui with end title
		/// </summary>
		/// <param name="end title"></param>
		public void Show(string endTitle)
		{
			endText.text = endTitle;
			Show();
		}
	}
}