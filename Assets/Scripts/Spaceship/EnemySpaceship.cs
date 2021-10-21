using System.Collections;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		private int destroyScore;
		private SoundKeys destroyedSound;

		public void SetDestroyScore(int destroyScore)
		{
			this.destroyScore = destroyScore;
		}

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
