using System.Collections;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class EnemySpaceship : Spaceship
	{
		private int destroyScore;
		private AudioManager audioManager;
		private SoundKeys destroyedSound;

		public void SetDestroyScore(int destroyScore)
		{
			this.destroyScore = destroyScore;
		}

		public void SetSounds(AudioManager audioManager, SoundKeys destroyedSound)
		{
			this.audioManager = audioManager;
		}

		private async void PlayDestroyedSound()
		{
			await this.audioManager?.PlaySound(destroyedSound);
		}

		protected override void ShipDestroy()
		{
			base.ShipDestroy();
			PlayDestroyedSound();
			//TODO: make it pool
			Destroy(this.gameObject);
		}
	}
}
