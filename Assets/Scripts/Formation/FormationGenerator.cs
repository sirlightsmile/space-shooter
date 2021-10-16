using System;
using System.Collections.Generic;
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

					Formation[] startFormation = new Formation[] { Formation.LinearOne, Formation.LinearTwo, Formation.LinearThree, Formation.LinearFour };
					List<Formation> formations = new List<Formation>() { startFormation[y] };
					// even or odd number
					if ((y == 0 && (x % 2 == 0)) || (y == 1 && (x % 2 != 0)))
					{
						formations.Add(Formation.UpperZigZag);
					}
					if ((y == 2 && (x % 2 == 0)) || (y == 3 && (x % 2 != 0)))
					{
						formations.Add(Formation.BottomZigzag);
					}
					if (((x == 1 || x == (width - 2)) && (y % 2 == 0)) || ((x == 0 || x == (width - 1)) && (y % 2 != 0)))
					{
						formations.Add(Formation.SideZigzag);
					}
					if ((x > 2 && x < 7) && (y > 0 && y < 3))
					{
						formations.Add(Formation.CenterGroup);
					}

					Formation result = formations.CombineFlags();
					point.SetFormation(result);
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
