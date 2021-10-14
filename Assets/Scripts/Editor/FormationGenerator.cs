using System;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	[ExecuteInEditMode]
	public class FormationGenerator : MonoBehaviour
	{
		public int width = 10;
		public int height = 4;
		public float widthInterval = 1f;
		public float heightInterval = 1f;
		public bool trigger = false;
		public bool triggerReset = false;
		public Transform container;

		void Update()
		{
			GenerateFormation();
			Reset();
		}

		[ContextMenu("GenerateFormation")]
		void GenerateFormation()
		{
			if (!trigger)
			{
				return;
			}
			trigger = false;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Vector2 startPos = Vector2.zero;
					GameObject obj = new GameObject($"FormationPoint_{x}_{y}");
					obj.transform.SetParent(container);
					FormationPoint point = obj.AddComponent<FormationPoint>();
					obj.transform.localPosition = new Vector2(startPos.x + (x * widthInterval), -(startPos.y + (y * heightInterval)));

					// even or odd number
					FormationType formation = FormationType.None;
					FormationType linerFormation = (FormationType)Enum.ToObject(typeof(FormationType), 1 << y);
					formation &= FormationType.LinerOne;

					point.SetFormationFlag(FormationType.LinerOne);
				}
			}
		}

		void Reset()
		{
			if (!triggerReset)
			{
				return;
			}
			triggerReset = false;

			for (int i = container.childCount - 1; i >= 0; i--)
			{
				DestroyImmediate(container.GetChild(i).gameObject);
			}
		}
	}
}
