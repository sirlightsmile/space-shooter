using System;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Bullet : PoolObject
	{
		private int damage;
		private float yBorder;
		private Spaceship owner;

		public override void OnSpawn() { }

		public override void OnDespawn() { }

		private void Start()
		{
			SetBorder();
		}

		public void SetDamage(int damage)
		{
			this.damage = damage;
		}

		public void SetOwner(Spaceship owner)
		{
			this.owner = owner;
		}

		private void FixedUpdate()
		{
			if (Vector2.Dot(transform.up, Vector2.down) > 0)
			{
				transform.Translate(-transform.up * Time.fixedDeltaTime * 5f);
			}
			else
			{
				transform.Translate(transform.up * Time.fixedDeltaTime * 5f);
			}
			if (IsVisible())
			{
				ReturnToPool();
			}
		}

		private bool IsVisible()
		{
			return transform.position.y > yBorder || transform.position.y < -yBorder;
		}

		/// <summary>
		/// Setup world y border for check visible
		/// </summary>
		private void SetBorder()
		{
			float borderY = Screen.height;
			float borderWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, borderY, 0)).y;
			yBorder = borderWorldPoint;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Spaceship spaceship = other.transform.GetComponent<Spaceship>();
			if (spaceship != null && spaceship.SpaceshipTag != owner.SpaceshipTag)
			{
				spaceship.GetHit(damage, owner);
				ReturnToPool();
			}
		}
	}
}