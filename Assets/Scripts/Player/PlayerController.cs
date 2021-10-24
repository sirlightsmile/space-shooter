using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PlayerController
	{
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

		private void SetPlayer(PlayerSpaceship player)
		{
			this.player = player;
			player.AttackSuccess += OnPlayerAttackSuccess;
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

		private void OnPlayerAttackSuccess(Spaceship player, Spaceship other)
		{
			if (other == null || other.IsBroken())
			{
				if (other.SpaceshipTag == SpaceshipTag.Enemy)
				{
					EnemySpaceship enemy = other.GetComponent<EnemySpaceship>();
					PlayerScore += enemy.DestroyScore;
				}
			}
		}

		private void OnPlayerDestroyed(Spaceship spaceship)
		{
			PlayerDestroyed?.Invoke();
		}
	}
}
