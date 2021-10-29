using System;
using System.Linq;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class FormationPoint : MonoBehaviour
	{
		[SerializeField, EnumFlag(EnumFlagAttribute.FlagLayout.List)]
		/// <summary>
		/// Formation type included this point
		/// </summary>
		private Formation formationTypes;

		/// <summary>
		/// Spaceship took this position
		/// </summary>
		private Spaceship spaceship;

		private void Start()
		{
			Debug.Assert(formationTypes.GetFlags<Formation>().Count() > 0, "Formation point should have at least one formation");
		}

		/// <summary>
		/// Set spaceship to landed in this formation point
		/// </summary>
		/// <param name="spaceship">Spaceship</param>
		public void SetLandedSpaceship(Spaceship spaceship)
		{
			if (HasLandedSpaceship())
			{
				Debug.LogError("FormationPoint already been token.");
				return;
			}
			this.spaceship = spaceship;
			this.spaceship.transform.SetParent(this.transform);
			this.spaceship.Destroyed += OnLandedShipDestroyed;
		}

		/// <summary>
		/// Check whether this formation point has landed spaceship
		/// </summary>
		/// <returns></returns>
		public bool HasLandedSpaceship()
		{
			return this.spaceship != null;
		}

		/// <summary>
		/// Get current formation assigned to the formation point
		/// </summary>
		/// <returns>formation flag</returns>
		public Formation GetFormations()
		{
			return this.formationTypes;
		}

		/// <summary>
		/// Set current formation assigned to the formation point
		/// </summary>
		/// <param name="formation">formation flag</param>
		public void SetFormation(Formation formation)
		{
			this.formationTypes = formation;
		}

		/// <summary>
		/// Get formation point position
		/// </summary>
		/// <returns>formation point position</returns>
		public Vector2 GetPosition()
		{
			return this.transform.position;
		}

		private void OnLandedShipDestroyed(Spaceship destroyedSpaceship)
		{
			destroyedSpaceship.Destroyed -= OnLandedShipDestroyed;
			if (this.spaceship != destroyedSpaceship)
			{
				Debug.LogError("Wrong landed ship destroy listener");
				return;
			}
			this.spaceship = null;
		}

#if UNITY_EDITOR
		[SerializeField]
		private FormationController controller;

		void OnDrawGizmos()
		{
			if (controller != null && controller.IsActiveFormation(this.formationTypes))
			{
				Gizmos.DrawWireSphere(this.transform.position, 0.5f);
			}
		}
#endif
	}
}
