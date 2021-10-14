using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class FormationPoint : MonoBehaviour
	{
		[EnumFlag(EnumFlagAttribute.FlagLayout.List)]
		/// <summary>
		/// Formation type included this point
		/// </summary>
		public FormationType formationTypes;

		[SerializeField]
		private FormationController controller;
		private bool isActive = false;

		void Start()
		{
			Debug.Assert(formationTypes != FormationType.None, "Formation point should have at least one formation");
			this.controller = FormationController.GetInstance();
		}

		private void SetActivePoint(bool isActive)
		{
			this.isActive = isActive;
		}

		public bool HasFormationType(FormationType formationType)
		{
			return formationTypes.HasFlag(formationType);
		}

		public void SetFormationFlag(FormationType formationType)
		{
			this.formationTypes = formationType;
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (controller.IsActiveFormation(this.formationTypes))
			{
				Gizmos.DrawWireSphere(this.transform.position, 0.5f);
			}
		}
	}
#endif
}
