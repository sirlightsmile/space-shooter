using System.Collections;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		private int destroyScore;

		public void SetDestroyScore(int destroyScore)
		{
			this.destroyScore = destroyScore;
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			//TODO: make it pool
			Destroy(this.gameObject);
		}
	}
}
