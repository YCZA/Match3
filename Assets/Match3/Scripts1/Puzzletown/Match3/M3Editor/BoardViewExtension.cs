using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Shared.DataStructures;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x020004FD RID: 1277
	public class BoardViewExtension : MonoBehaviour
	{
		// Token: 0x06002320 RID: 8992 RVA: 0x0009BA2C File Offset: 0x00099E2C
		public void DisplayFieldViewExtension(Field field, FieldView fieldView, BoardView boardView, Fields fields)
		{
			Transform transform = fieldView.transform;
			fieldView.spriteTile.color = this.semiTransparent;
			fieldView.spriteCrate.color = this.semiTransparent;
			Color color = fieldView.colorCrateSprite.color;
			color.a = this.semiTransparent.a;
			fieldView.colorCrateSprite.color = color;
			bool flag = field.gem.color == GemColor.Treasure && field.gem.modifier == GemModifier.DirtHp3;
			if (flag && this.generalIndicators[field.gridPosition] == null)
			{
				Transform transform2 = boardView.GetGemView(field.gridPosition, true).transform;
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.treasureSprite, transform2);
				gameObject.transform.localPosition = Vector3.zero;
				this.generalIndicators[field.gridPosition] = gameObject;
			}
			else if (!flag && this.generalIndicators[field.gridPosition] != null)
			{
				global::UnityEngine.Object.Destroy(this.generalIndicators[field.gridPosition]);
			}
			bool flag2 = field.CanSpawnInTrickling || field.IsDefinedGemSpawner || field.portalId != 0 || field.CanSpawnClimber || field.CanSpawnChameleons;
			if (flag2 && !this.spawningIndicators.ContainsKey(field))
			{
				this.spawningIndicators.Add(field, this.InstantiateIndicator(transform));
			}
			if (this.spawningIndicators.ContainsKey(field))
			{
				this.spawningIndicators[field].DisplayFieldForEditor(field);
			}
			bool flag3 = field.IsColorWheel && fields.GetColorWheelCorner(field.gridPosition) == default(IntVector2);
			if (flag3 && this.generalIndicators[field.gridPosition] == null)
			{
				Transform transform3 = boardView.GetFieldView(field.gridPosition).transform;
				GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.brokenColorWheel, transform3);
				gameObject2.transform.localPosition = Vector3.zero;
				this.generalIndicators[field.gridPosition] = gameObject2;
			}
			else if (!flag3 && this.generalIndicators[field.gridPosition] != null)
			{
				global::UnityEngine.Object.Destroy(this.generalIndicators[field.gridPosition]);
			}
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0009BCAC File Offset: 0x0009A0AC
		public void DisplayRandomHiddenItems(BoardView boardView)
		{
			foreach (KeyValuePair<int, HiddenItemView> keyValuePair in boardView.HiddenItemViews)
			{
				if (keyValuePair.Key > 4)
				{
					keyValuePair.Value.MarkAsRandom();
				}
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0009BD18 File Offset: 0x0009A118
		public void RemoveHiddenItemViews(BoardView boardView)
		{
			foreach (KeyValuePair<int, HiddenItemView> keyValuePair in boardView.HiddenItemViews)
			{
				global::UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
			}
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0009BD7C File Offset: 0x0009A17C
		public void RemoveColorWheels(BoardView boardView)
		{
			foreach (KeyValuePair<IntVector2, ColorWheelView> keyValuePair in boardView.ColorWheelViews)
			{
				if (keyValuePair.Value != null)
				{
					global::UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0009BDF4 File Offset: 0x0009A1F4
		public void RemoveIndicators()
		{
			foreach (KeyValuePair<Field, FieldViewIndicator> keyValuePair in this.spawningIndicators)
			{
				global::UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.spawningIndicators.Clear();
			IEnumerator enumerator2 = this.generalIndicators.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj = enumerator2.Current;
					GameObject obj2 = (GameObject)obj;
					global::UnityEngine.Object.Destroy(obj2);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator2 as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.generalIndicators.Clear();
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0009BEC8 File Offset: 0x0009A2C8
		private FieldViewIndicator InstantiateIndicator(Transform parent)
		{
			FieldViewIndicator fieldViewIndicator = global::UnityEngine.Object.Instantiate<FieldViewIndicator>(this.indicator);
			fieldViewIndicator.transform.SetParent(parent);
			fieldViewIndicator.transform.localPosition = Vector3.zero;
			fieldViewIndicator.transform.localScale = Vector3.one;
			return fieldViewIndicator;
		}

		// Token: 0x04004ED6 RID: 20182
		[SerializeField]
		private FieldViewIndicator indicator;

		// Token: 0x04004ED7 RID: 20183
		[SerializeField]
		private Color semiTransparent = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x04004ED8 RID: 20184
		[SerializeField]
		private GameObject treasureSprite;

		// Token: 0x04004ED9 RID: 20185
		[SerializeField]
		private GameObject brokenColorWheel;

		// Token: 0x04004EDA RID: 20186
		private readonly Dictionary<Field, FieldViewIndicator> spawningIndicators = new Dictionary<Field, FieldViewIndicator>();

		// Token: 0x04004EDB RID: 20187
		private readonly Map<GameObject> generalIndicators = new Map<GameObject>(9);
	}
}
