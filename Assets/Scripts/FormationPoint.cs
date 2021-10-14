using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	public class FormationPoint : MonoBehaviour
	{
		[SerializeField, EnumFlag(EnumFlagAttribute.FlagLayout.List)]
		/// <summary>
		/// Formation type included this point
		/// </summary>
		private FormationType formationTypes;

		[SerializeField]
		private FormationController controller;

		void Start()
		{
			Debug.Assert(formationTypes != FormationType.None, "Formation point should have at least one formation");
			this.controller = FormationController.GetInstance();
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (controller != null && controller.IsActiveFormation(this.formationTypes))
			{
				Gizmos.DrawWireSphere(this.transform.position, 0.5f);
			}
		}
	}
#endif
}
