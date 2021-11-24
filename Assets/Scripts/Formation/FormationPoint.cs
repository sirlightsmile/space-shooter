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
		private Formation _formationTypes;

		/// <summary>
		/// Spaceship took this position
		/// </summary>
		private Spaceship _spaceship;

		private void Start()
		{
			Debug.Assert(_formationTypes.GetFlags<Formation>().Count() > 0, "Formation point should have at least one formation");
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
			_spaceship = spaceship;
			_spaceship.transform.SetParent(this.transform);
			_spaceship.Destroyed += OnLandedShipDestroyed;
		}

		/// <summary>
		/// Check whether this formation point has landed spaceship
		/// </summary>
		/// <returns></returns>
		public bool HasLandedSpaceship()
		{
			return _spaceship != null;
		}

		/// <summary>
		/// Get current formation assigned to the formation point
		/// </summary>
		/// <returns>formation flag</returns>
		public Formation GetFormations()
		{
			return _formationTypes;
		}

		/// <summary>
		/// Set current formation assigned to the formation point
		/// </summary>
		/// <param name="formation">formation flag</param>
		public void SetFormation(Formation formation)
		{
			_formationTypes = formation;
		}
		private void OnLandedShipDestroyed(Spaceship destroyedSpaceship)
		{
			destroyedSpaceship.Destroyed -= OnLandedShipDestroyed;
			if (_spaceship != destroyedSpaceship)
			{
				Debug.LogError("Wrong landed ship destroy listener");
				return;
			}
			_spaceship = null;
		}

#if UNITY_EDITOR
		[SerializeField]
		private FormationController controller;

		void OnDrawGizmos()
		{
			if (controller != null && controller.IsActiveFormation(_formationTypes))
			{
				Gizmos.DrawWireSphere(this.transform.position, 0.5f);
			}
		}
#endif
	}
}
