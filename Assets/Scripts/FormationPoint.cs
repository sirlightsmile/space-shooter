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
		private FormationType formationTypes;

		private void Start()
		{
			Debug.Assert(formationTypes.GetFlags<FormationType>().Count() > 0, "Formation point should have at least one formation");
		}

		public FormationType GetFormations()
		{
			return this.formationTypes;
		}

		public Vector2 GetPosition()
		{
			return this.transform.position;
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
	}
#endif
}
