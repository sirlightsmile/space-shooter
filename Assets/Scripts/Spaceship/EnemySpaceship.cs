using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		public override SpaceshipTag SpaceshipTag { get { return SpaceshipTag.Enemy; } }
		public int DestroyScore { get; private set; }
		private SoundKeys destroyedSound;

		public void SetDestroyScore(int destroyScore)
		{
			this.DestroyScore = destroyScore;
		}

		//TODO: on hit sound
		public void SetSounds(AudioManager audioManager, SoundKeys destroyedSound)
		{
			this.audioManager = audioManager;
			this.destroyedSound = destroyedSound;
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
