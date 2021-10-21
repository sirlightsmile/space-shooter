﻿using SmileProject.Generic;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		private int destroyScore;
		private SoundKeys destroyedSound;

		//TODO: implement enemy spaceship attack formation

		public void SetDestroyScore(int destroyScore)
		{
			this.destroyScore = destroyScore;
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
