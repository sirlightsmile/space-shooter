using System;
using System.Threading.Tasks;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class PlayerController
	{
		public Action PlayerDestroyed;
		private PlayerSpaceship player;

		public PlayerController(ISpaceShooterInput inputManager)
		{
			inputManager.AttackInput += PlayerShoot;
			inputManager.HorizontalInput += PlayerMove;
		}

		public PlayerSpaceshipBuilder CreatePlayerBuilder(IResourceLoader resourceLoader, WeaponFactory weaponFactory, GameDataManager gameDataManager)
		{
			return new PlayerSpaceshipBuilder(resourceLoader, gameDataManager, weaponFactory);
		}

		public async Task<PlayerSpaceship> CreatePlayer(Vector2 spawnPoint, PlayerSpaceshipBuilder builder)
		{
			PlayerSpaceship player = await builder.BuildRandomSpaceship();
			player.SetPosition(spawnPoint);
			SetPlayer(player);
			return player;
		}

		public void SetPlayer(PlayerSpaceship player)
		{
			this.player = player;
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

		private void OnPlayerDestroyed(Spaceship spaceship)
		{
			PlayerDestroyed?.Invoke();
		}
	}
}
