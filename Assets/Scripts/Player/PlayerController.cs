using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PlayerController
	{
		/// <summary>
		/// Invoke when player get hit with current hp
		/// </summary>
		public Action<int> PlayerGetHit;

		/// <summary>
		/// Invoke when player destroyed
		/// </summary>
		public Action PlayerDestroyed;

		public int PlayerScore { get; private set; } = 0;
		private PlayerSpaceship player;
		private PlayerSpaceshipBuilder builder;

		public PlayerController(ISpaceShooterInput inputManager, PlayerSpaceshipBuilder builder)
		{
			this.builder = builder;
			inputManager.AttackInput += PlayerShoot;
			inputManager.HorizontalInput += PlayerMove;
		}

		public async Task<PlayerSpaceship> CreatePlayer(Vector2 spawnPoint)
		{
			PlayerSpaceship player = await builder.BuildRandomSpaceship();
			player.SetPosition(spawnPoint);
			SetPlayer(player);
			return player;
		}

		public void AddScore(int score)
		{
			PlayerScore += score;
		}

		private void SetPlayer(PlayerSpaceship player)
		{
			this.player = player;
			player.GotHit += OnPlayerGotHit;
			player.Destroyed += OnPlayerDestroyed;
		}

		private void PlayerShoot()
		{
			player?.Shoot();
		}


		private void PlayerMove(MoveDirection moveDirection)
		{
			player?.MoveToDirection(moveDirection);
		}

		private void OnPlayerGotHit(Spaceship other, Spaceship player)
		{
			int currentHp = player != null ? player.HP : 0;
			PlayerGetHit?.Invoke(currentHp);
		}

		private void OnPlayerDestroyed(Spaceship spaceship)
		{
			PlayerDestroyed?.Invoke();
		}
	}
}
