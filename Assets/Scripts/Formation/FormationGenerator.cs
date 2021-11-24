#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace SmileProject.SpaceShooter
{
	/// <summary>
	/// This is class for help generate formation object as plan to set on prefab
	/// use in development only
	/// </summary>
	[ExecuteInEditMode]
	public sealed class FormationGenerator : MonoBehaviour
	{
		[SerializeField]
		private int _width = 10;

		[SerializeField]
		private int _height = 4;

		[SerializeField]
		private float _widthInterval = 1f;

		[SerializeField]
		private float _heightInterval = 1f;

		[SerializeField]
		private Transform _container;


		[ContextMenu("GenerateFormation")]
		private void GenerateFormation()
		{
			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					Vector2 startPos = Vector2.zero;
					GameObject obj = new GameObject($"FormationPoint_{x}_{y}");
					obj.transform.SetParent(_container);
					FormationPoint point = obj.AddComponent<FormationPoint>();
					obj.transform.localPosition = new Vector2(startPos.x + (x * _widthInterval), -(startPos.y + (y * _heightInterval)));

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
					if (((x == 1 || x == (_width - 2)) && (y % 2 == 0)) || ((x == 0 || x == (_width - 1)) && (y % 2 != 0)))
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

		[ContextMenu("Reset")]
		void Reset()
		{
			for (int i = _container.childCount - 1; i >= 0; i--)
			{
				DestroyImmediate(_container.GetChild(i).gameObject);
			}
		}
	}
}
#endif