using System;
using System.Collections.Generic;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200068F RID: 1679
	public class BoardBorderFactory : MonoBehaviour, IDataView<LevelTheme>
	{
		// Token: 0x060029B9 RID: 10681 RVA: 0x000BDD71 File Offset: 0x000BC171
		public void Show(LevelTheme theme)
		{
			this.ReuseMaterial(BoardBorderFactory.MaterialSlot.Board, theme.board);
			this.ReuseMaterial(BoardBorderFactory.MaterialSlot.InnerBorder, theme.innerBorder);
			this.ReuseMaterial(BoardBorderFactory.MaterialSlot.OuterBorder, theme.outerBorder);
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x000BDD9C File Offset: 0x000BC19C
		private void ReuseMaterial(BoardBorderFactory.MaterialSlot slot, Texture2D texture)
		{
			if (texture == null)
			{
				return;
			}
			this.materials[(int)slot] = new Material(this.materials[(int)slot])
			{
				mainTexture = texture
			};
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000BDDD4 File Offset: 0x000BC1D4
		public void GenerateBorder(Fields fields)
		{
			if (this.cachedObject)
			{
				global::UnityEngine.Object.Destroy(this.cachedObject);
			}
			this.workspace = new Map<bool>(fields.size);
			Mesh mesh = this.CombineMeshes("Inner", this.PopulateBorder(fields, new Func<Fields, IntVector2, bool>(this.CheckInnerField)), true);
			Mesh mesh2 = this.CombineMeshes("Outer", this.PopulateBorder(fields, new Func<Fields, IntVector2, bool>(this.CheckOuterField)), true);
			// for (int i = 0; i < 16; i++)
			// {
			// 	GameObject go = new GameObject("meshtest" + i);
			// 	go.AddComponent<MeshFilter>().mesh=CreateTileMesh(i, new IntVector2(1, 1));
			// 	go.AddComponent<MeshRenderer>();
			// 	go.transform.position = new Vector3(i*1.2f, 0);
			// }
			//
			// GameObject go2 = new GameObject("meshtest");
			// go2.AddComponent<MeshFilter>().mesh = mesh2;
			this.cachedObject = this.CreateGameObject("Border", new Mesh[]
			{
				this.PopulateBoard(fields),
				mesh,
				mesh2
			});
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000BDE78 File Offset: 0x000BC278
		private Mesh PopulateBoard(Fields fields)
		{
			int num = (fields.size + 1) * (fields.size + 1);
			Vector3[] array = new Vector3[num];
			Vector2[] array2 = new Vector2[num];
			List<int> list = new List<int>();
			int i = 0;
			int num2 = 0;
			while (i <= fields.size)
			{
				int j = 0;
				while (j <= fields.size)
				{
					array[num2] = new Vector3((float)(i + 1), (float)(j + 1), 0f);
					array2[num2] = new Vector2((float)i / (float)fields.size, (float)j / (float)fields.size);
					j++;
					num2++;
				}
				i++;
			}
			for (int k = 0; k < fields.size; k++)
			{
				for (int l = 0; l < fields.size; l++)
				{
					if (fields[k, l].isOn)
					{
						list.Add(k * (fields.size + 1) + l);
						list.Add(k * (fields.size + 1) + l + 1);
						list.Add((k + 1) * (fields.size + 1) + l + 1);
						list.Add((k + 1) * (fields.size + 1) + l + 1);
						list.Add((k + 1) * (fields.size + 1) + l);
						list.Add(k * (fields.size + 1) + l);
					}
				}
			}
			return new Mesh
			{
				name = "Board",
				vertices = array,
				uv = array2,
				triangles = list.ToArray()
			};
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000BE034 File Offset: 0x000BC434
		private Mesh[] PopulateBorder(Fields fields, Func<Fields, IntVector2, bool> func)
		{
			List<Mesh> list = new List<Mesh>();
			for (int i = 0; i <= fields.size; i++)
			{
				for (int j = 0; j <= fields.size; j++)
				{
					IntVector2 pos = new IntVector2(i, j);
					int num = this.CalculateIndexAt(fields, func, pos);
					if (num != 0)
					{
						list.Add(this.CreateTileMesh(num, pos));
					}
				}
			}
			if (list.Count == 0)
			{
				list.Add(new Mesh
				{
					vertices = new Vector3[3],
					uv = new Vector2[3],
					triangles = new int[3]
				});
			}
			return list.ToArray();
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000BE0EC File Offset: 0x000BC4EC
		private GameObject CreateGameObject(string name, params Mesh[] meshes)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.AddComponent<MeshFilter>().mesh = this.CombineMeshes("Border", meshes, false);
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterials = this.materials;
			meshRenderer.sortingLayerName = this.sortingLayer;
			meshRenderer.sortingOrder = this.sortingOrder;
			gameObject.transform.localPosition = new Vector2(-1f + BoardView.fieldOffset, -1f + BoardView.fieldOffset);
			gameObject.transform.SetParent(base.transform, false);
			return gameObject;
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000BE184 File Offset: 0x000BC584
		private Mesh CombineMeshes(string name, Mesh[] meshes, bool mergeSubmeshes)
		{
			Mesh mesh = new Mesh
			{
				name = name
			};
			CombineInstance[] combine = Array.ConvertAll<Mesh, CombineInstance>(meshes, (Mesh m) => new CombineInstance
			{
				mesh = m
			});
			mesh.CombineMeshes(combine, mergeSubmeshes, false);
			return mesh;
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000BE1D0 File Offset: 0x000BC5D0
		private Mesh CreateTileMesh(int config, IntVector2 pos)
		{
			Vector3[] array = new Vector3[]
			{
				new Vector3((float)pos.x, (float)pos.y, 0f),
				new Vector3((float)pos.x, (float)(pos.y + 1), 0f),
				new Vector3((float)(pos.x + 1), (float)(pos.y + 1), 0f),
				new Vector3((float)(pos.x + 1), (float)pos.y, 0f)
			};
			float num = 0.025f;
			float num2 = 1f - num;
			Vector2[] array2 = new Vector2[]
			{
				new Vector2(num, num2),
				new Vector2(num, num),
				new Vector2(num2, num),
				new Vector2(num2, num2)
			};
			int[] triangles = new int[]
			{
				0,
				1,
				2,
				0,
				2,
				3
			};
			array = Array.ConvertAll<Vector3, Vector3>(array, (Vector3 v) => new Vector3(v.x + 0.5f, v.y + 0.5f, 0f));
			int uvx = config % 4;
			int uvy = config / 4;
			array2 = Array.ConvertAll<Vector2, Vector2>(array2, (Vector2 v) => new Vector2(v.x + (float)uvx, 4f - (v.y + (float)uvy)) / 4f);
			return new Mesh
			{
				vertices = array,
				uv = array2,
				triangles = triangles
			};
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000BE370 File Offset: 0x000BC770
		private int CalculateIndexAt(Fields fields, Func<Fields, IntVector2, bool> func, IntVector2 pos)
		{
			int num = 0;
			// func函数检查第二个参数所对应的位置是不是innerField
			if (func(fields, pos + IntVector2.Left))
			{
				num |= 2;
			}
			if (func(fields, pos + IntVector2.Left + IntVector2.Down))
			{
				num |= 1;
			}
			if (func(fields, pos + IntVector2.Down))
			{
				num |= 8;
			}
			if (func(fields, pos))
			{
				num |= 4;
			}
			return num;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000BE3EC File Offset: 0x000BC7EC
		private bool CheckOuterField(Fields fields, IntVector2 pos)
		{
			this.workspace.Clear();
			return BoardBorderFactory.IsOuterField(this.workspace, fields, pos);
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000BE408 File Offset: 0x000BC808
		private bool CheckInnerField(Fields fields, IntVector2 pos)
		{
			Field field = fields[pos];
			this.workspace.Clear();
			return !BoardBorderFactory.IsOuterField(this.workspace, fields, pos) && !field.isOn;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000BE448 File Offset: 0x000BC848
		public static bool IsOuterField(Map<bool> map, Fields fields, IntVector2 pos)
		{
			Field field = fields[pos];
			if (field == null)
			{
				return true;
			}
			if (map[pos] || field.isOn)
			{
				return false;
			}
			map[pos] = true;
			foreach (IntVector2 b in IntVector2.Sides)
			{
				if (BoardBorderFactory.IsOuterField(map, fields, pos + b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005338 RID: 21304
		private const int TEXTURE_SIZE = 4;

		// Token: 0x04005339 RID: 21305
		[SerializeField]
		private Material[] materials;

		// Token: 0x0400533A RID: 21306
		private GameObject cachedObject;

		// Token: 0x0400533B RID: 21307
		private Map<bool> workspace;

		// Token: 0x0400533C RID: 21308
		public string sortingLayer = "Highlight";

		// Token: 0x0400533D RID: 21309
		public int sortingOrder = 10;

		// Token: 0x02000690 RID: 1680
		private enum MaterialSlot
		{
			// Token: 0x04005341 RID: 21313
			Board,
			// Token: 0x04005342 RID: 21314
			InnerBorder,
			// Token: 0x04005343 RID: 21315
			OuterBorder
		}
	}
}
