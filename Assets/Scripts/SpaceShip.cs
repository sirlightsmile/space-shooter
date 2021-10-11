using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public abstract class SpaceShip : MonoBehaviour
	{
		[SerializeField]
		protected int hp;

		[SerializeField]
		protected float speed;

		[SerializeField]
		protected float atk;

		public SpaceShip(int hp, float speed, float atk)
		{
			this.hp = hp;
			this.speed = speed;
			this.atk = atk;
		}

		public abstract void GetHit();

		protected abstract void OnShipDestroy();
	}
}
