using System;
using System.Collections.Generic;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000692 RID: 1682
	public class BoardDecorationsFactory : MonoBehaviour, IDataView<LevelTheme>
	{
		// Token: 0x060029CE RID: 10702 RVA: 0x000BE762 File Offset: 0x000BCB62
		public void Show(LevelTheme theme)
		{
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000BE764 File Offset: 0x000BCB64
		private void GenerateElements()
		{
			if (this.masks.Length != this.images.Length)
			{
				global::UnityEngine.Debug.LogWarningFormat(this, "Deco masks ({0}) and images ({1}) don't have the same length!", new object[]
				{
					this.masks.Length,
					this.images.Length
				});
			}
			int num = Mathf.Min(this.masks.Length, this.images.Length);
			this.elements = new DecorationElement[num];
			for (int i = 0; i < num; i++)
			{
				this.elements[i] = new DecorationElement
				{
					sprite = this.images[i],
					config = this.masks[i]
				};
			}
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000BE818 File Offset: 0x000BCC18
		public void GenerateDecorations(Fields fields)
		{
			int num = 0;
			Map<bool> availableFields = this.GetAvailableFields(fields, out num);
			num /= 3;
			if (this.elements == null)
			{
				this.GenerateElements();
			}
			for (int i = 0; i < num; i++)
			{
				List<KeyValuePair<IntVector2, DecorationElement>> list = new List<KeyValuePair<IntVector2, DecorationElement>>();
				for (int j = 0; j < fields.size; j++)
				{
					for (int k = 0; k < fields.size; k++)
					{
						IntVector2 pos = new IntVector2(k, j);
						int mask = DecorationElement.GenerateMask(availableFields, pos);
						DecorationElement[] array = Array.FindAll<DecorationElement>(this.elements, (DecorationElement e) => (e.Mask & mask) == e.Mask);
						list.AddRange(Array.ConvertAll<DecorationElement, KeyValuePair<IntVector2, DecorationElement>>(array, (DecorationElement e) => new KeyValuePair<IntVector2, DecorationElement>(pos, e)));
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				KeyValuePair<IntVector2, DecorationElement> keyValuePair = RandomHelper.Next<KeyValuePair<IntVector2, DecorationElement>>(list);
				this.CreateDecoration(keyValuePair.Value, keyValuePair.Key);
				bool[,] map = keyValuePair.Value.Map;
				for (int l = 0; l < keyValuePair.Value.height; l++)
				{
					for (int m = 0; m < keyValuePair.Value.width; m++)
					{
						Map<bool> map2;
						int x;
						int y;
						(map2 = availableFields)[x = m + keyValuePair.Key.x, y = l + keyValuePair.Key.y] = (map2[x, y] & !map[m, l]);
					}
				}
			}
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000BE9C0 File Offset: 0x000BCDC0
		private Map<bool> GetAvailableFields(Fields fields, out int count)
		{
			Map<bool> map = new Map<bool>(fields.size);
			Map<bool> map2 = new Map<bool>(fields.size);
			int num = 0;
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					map2.Clear();
					IntVector2 intVector = new IntVector2(j, i);
					if (BoardBorderFactory.IsOuterField(map2, fields, intVector))
					{
						map[intVector] = true;
						num++;
					}
				}
			}
			count = num;
			return map;
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000BEA48 File Offset: 0x000BCE48
		private GameObject CreateDecoration(DecorationElement el, IntVector2 position)
		{
			GameObject gameObject = new GameObject(el.sprite.name);
			gameObject.transform.localPosition = new Vector3((float)position.x + BoardView.fieldOffset, (float)position.y + BoardView.fieldOffset, -0.1f);
			gameObject.transform.SetParent(base.transform, false);
			Vector2 b = new Vector2(1f / (float)el.sprite.texture.width, 1f / (float)el.sprite.texture.height);
			Vector2 b2 = new Vector2((float)el.width / el.sprite.rect.width, (float)el.height / el.sprite.rect.height);
			Rect rect = new Rect(Vector2.Scale(el.sprite.textureRect.min, b), Vector2.Scale(el.sprite.textureRect.size, b));
			Rect rect2 = new Rect(Vector2.Scale(el.sprite.textureRectOffset, b2), Vector2.Scale(el.sprite.textureRect.size, b2));
			Mesh mesh = new Mesh
			{
				name = el.sprite.name,
				vertices = new Vector3[]
				{
					new Vector3(rect2.min.x, rect2.min.y),
					new Vector3(rect2.min.x, rect2.max.y),
					new Vector3(rect2.max.x, rect2.max.y),
					new Vector3(rect2.max.x, rect2.min.y)
				},
				uv = new Vector2[]
				{
					new Vector2(rect.min.x, rect.min.y),
					new Vector2(rect.min.x, rect.max.y),
					new Vector2(rect.max.x, rect.max.y),
					new Vector2(rect.max.x, rect.min.y)
				},
				triangles = new int[]
				{
					0,
					1,
					2,
					0,
					2,
					3
				}
			};
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			gameObject.AddComponent<MeshFilter>().mesh = mesh;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetTexture("_MainTex", el.sprite.texture);
			meshRenderer.material = this.material;
			meshRenderer.SetPropertyBlock(materialPropertyBlock);
			meshRenderer.sortingOrder = this.sortingOrder;
			meshRenderer.sortingLayerName = this.sortingLayer;
			return gameObject;
		}

		// Token: 0x04005348 RID: 21320
		public Sprite[] masks;

		// Token: 0x04005349 RID: 21321
		public Sprite[] images;

		// Token: 0x0400534A RID: 21322
		public Material material;

		// Token: 0x0400534B RID: 21323
		private DecorationElement[] elements;

		// Token: 0x0400534C RID: 21324
		public string sortingLayer = "FieldsOverlay";

		// Token: 0x0400534D RID: 21325
		public int sortingOrder = 10;

		// Token: 0x0400534E RID: 21326
		private const int FIELDS_PER_DECO = 3;

		// Token: 0x0400534F RID: 21327
		private const int TEXTURE_SIZE = 4;
	}
}
