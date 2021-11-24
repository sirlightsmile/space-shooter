using System;
using SmileProject.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class Bullet : PoolObject
	{
		private int _damage;
		private float _yBorder;
		private Spaceship _owner;

		public override void OnSpawn() { }

		public override void OnDespawn() { }

		private void Start()
		{
			SetBorder();
		}

		public Bullet SetDamage(int damage)
		{
			_damage = damage;
			return this;
		}

		public Bullet SetOwner(Spaceship owner)
		{
			_owner = owner;
			return this;
		}

		public Bullet SetPosition(Vector2 position)
		{
			this.transform.position = position;
			return this;
		}

		public Bullet SetRotation(Quaternion rotation)
		{
			this.transform.rotation = rotation;
			return this;
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
			return transform.position.y > _yBorder || transform.position.y < -_yBorder;
		}

		/// <summary>
		/// Setup world y border for check visible
		/// </summary>
		private void SetBorder()
		{
			float borderY = Screen.height;
			float borderWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, borderY, 0)).y;
			_yBorder = borderWorldPoint;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Spaceship spaceship = other.transform.GetComponent<Spaceship>();
			if (spaceship != null && spaceship.SpaceshipTag != _owner.SpaceshipTag)
			{
				spaceship.GetHit(_damage, _owner);
				ReturnToPool();
			}
		}
	}
}