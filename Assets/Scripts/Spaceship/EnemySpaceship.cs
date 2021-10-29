using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		public override SpaceshipTag SpaceshipTag { get { return SpaceshipTag.Enemy; } }
		public int DestroyScore { get; private set; }

		public void SetDestroyScore(int destroyScore)
		{
			this.DestroyScore = destroyScore;
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			PlaySound(destroyedSound);
			//TODO: make it pool
			Destroy(this.gameObject);
		}
	}
}
