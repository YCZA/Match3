using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B41 RID: 2881
	public class ListView : ATableView
	{
		// Token: 0x06004384 RID: 17284 RVA: 0x001585A8 File Offset: 0x001569A8
		public override void Reload()
		{
			foreach (ATableViewReusableCell atableViewReusableCell in this.activeCells)
			{
				base.AddReusableCellToPool(atableViewReusableCell);
				atableViewReusableCell.gameObject.SetActive(false);
			}
			this.activeCells.Clear();
			if (base.DataSource == null)
			{
				return;
			}
			for (int i = 0; i < base.DataSource.GetNumberOfCellsForTableView(); i++)
			{
				this.AddCellForIndex(i, this.delayPerElement == 0f);
			}
			if (this.sortByReusableId)
			{
				this.SortByReusableId();
			}
			if (this.delayPerElement > 0f)
			{
				if (this.elementsAnimation != null)
				{
					WooroutineRunner.Stop(this.elementsAnimation);
				}
				this.elementsAnimation = WooroutineRunner.StartCoroutine(this.AnimateActivation(), null);
			}
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x001586A0 File Offset: 0x00156AA0
		public override IEnumerable<ATableViewReusableCell> GetAllCells()
		{
			return this.activeCells;
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x001586A8 File Offset: 0x00156AA8
		public override IEnumerable<ATableViewReusableCell> GetVisibleCells(RectTransform viewport, RectTransform slider)
		{
			List<ATableViewReusableCell> list = new List<ATableViewReusableCell>();
			VisibilityHelper visibilityHelper = new VisibilityHelper(viewport);
			foreach (ATableViewReusableCell atableViewReusableCell in this.activeCells)
			{
				if (visibilityHelper.IsVisible(atableViewReusableCell.GetComponent<RectTransform>()))
				{
					list.Add(atableViewReusableCell);
				}
			}
			return list;
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x00158724 File Offset: 0x00156B24
		private void AddCellForIndex(int i, bool setActive)
		{
			ATableViewReusableCell cellForIndex = base.DataSource.GetCellForIndex(i, base.transform);
			if (cellForIndex == null)
			{
				return;
			}
			if (cellForIndex.transform.parent == null)
			{
				cellForIndex.transform.SetParent(base.transform);
			}
			cellForIndex.transform.SetAsLastSibling();
			cellForIndex.transform.localPosition = Vector3.zero;
			cellForIndex.transform.localScale = Vector3.one;
			cellForIndex.gameObject.SetActive(setActive);
			this.activeCells.Add(cellForIndex);
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x001587BC File Offset: 0x00156BBC
		private IEnumerator AnimateActivation()
		{
			foreach (ATableViewReusableCell e in this.activeCells)
			{
				e.gameObject.SetActive(true);
				yield return new WaitForSeconds(this.delayPerElement);
			}
			this.elementsAnimation = null;
			yield break;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x001587D8 File Offset: 0x00156BD8
		private void SortByReusableId()
		{
			Dictionary<ATableViewReusableCell, int> order = new Dictionary<ATableViewReusableCell, int>();
			ATableViewReusableCell[] componentsInChildren = base.GetComponentsInChildren<ATableViewReusableCell>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				order[componentsInChildren[i]] = i + componentsInChildren[i].reusableId * 10000;
			}
			Array.Sort<ATableViewReusableCell>(componentsInChildren, (ATableViewReusableCell x, ATableViewReusableCell y) => order[x].CompareTo(order[y]));
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].transform.SetSiblingIndex(j);
			}
		}

		// Token: 0x04006BF4 RID: 27636
		public readonly List<ATableViewReusableCell> activeCells = new List<ATableViewReusableCell>();

		// Token: 0x04006BF5 RID: 27637
		public bool sortByReusableId;

		// Token: 0x04006BF6 RID: 27638
		public float delayPerElement;

		// Token: 0x04006BF7 RID: 27639
		private Coroutine elementsAnimation;
	}
}
