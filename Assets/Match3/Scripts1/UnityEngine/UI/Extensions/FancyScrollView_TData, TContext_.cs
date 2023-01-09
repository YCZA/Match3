using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BE4 RID: 3044
	public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class
	{
		// Token: 0x0600476E RID: 18286 RVA: 0x0016C719 File Offset: 0x0016AB19
		protected void Awake()
		{
			this.cellBase.SetActive(false);
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x0016C728 File Offset: 0x0016AB28
		protected void SetContext(TContext context)
		{
			this.context = context;
			for (int i = 0; i < this.cells.Count; i++)
			{
				this.cells[i].SetContext(context);
			}
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x0016C76C File Offset: 0x0016AB6C
		private FancyScrollViewCell<TData, TContext> CreateCell()
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.cellBase);
			gameObject.SetActive(true);
			FancyScrollViewCell<TData, TContext> component = gameObject.GetComponent<FancyScrollViewCell<TData, TContext>>();
			RectTransform rectTransform = component.transform as RectTransform;
			Vector3 localScale = component.transform.localScale;
			Vector2 sizeDelta = Vector2.zero;
			Vector2 offsetMin = Vector2.zero;
			Vector2 offsetMax = Vector2.zero;
			if (rectTransform)
			{
				sizeDelta = rectTransform.sizeDelta;
				offsetMin = rectTransform.offsetMin;
				offsetMax = rectTransform.offsetMax;
			}
			component.transform.SetParent(this.cellBase.transform.parent);
			component.transform.localScale = localScale;
			if (rectTransform)
			{
				rectTransform.sizeDelta = sizeDelta;
				rectTransform.offsetMin = offsetMin;
				rectTransform.offsetMax = offsetMax;
			}
			component.SetContext(this.context);
			component.SetVisible(false);
			return component;
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x0016C844 File Offset: 0x0016AC44
		private void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex)
		{
			if (this.loop)
			{
				dataIndex = this.GetLoopIndex(dataIndex, this.cellData.Count);
			}
			else if (dataIndex < 0 || dataIndex > this.cellData.Count - 1)
			{
				cell.SetVisible(false);
				return;
			}
			cell.SetVisible(true);
			cell.DataIndex = dataIndex;
			cell.UpdateContent(this.cellData[dataIndex]);
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0016C8B7 File Offset: 0x0016ACB7
		private int GetLoopIndex(int index, int length)
		{
			if (index < 0)
			{
				index = length - 1 + (index + 1) % length;
			}
			else if (index > length - 1)
			{
				index %= length;
			}
			return index;
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0016C8DF File Offset: 0x0016ACDF
		protected void UpdateContents()
		{
			this.UpdatePosition(this.currentPosition);
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0016C8F0 File Offset: 0x0016ACF0
		protected void UpdatePosition(float position)
		{
			this.currentPosition = position;
			float num = position - this.cellOffset / this.cellInterval;
			float num2 = (Mathf.Ceil(num) - num) * this.cellInterval;
			int num3 = Mathf.CeilToInt(num);
			int i = 0;
			float num4 = num2;
			while (num4 <= 1f)
			{
				if (i >= this.cells.Count)
				{
					this.cells.Add(this.CreateCell());
				}
				num4 += this.cellInterval;
				i++;
			}
			i = 0;
			int loopIndex;
			for (float num5 = num2; num5 <= 1f; num5 += this.cellInterval)
			{
				int num6 = num3 + i;
				loopIndex = this.GetLoopIndex(num6, this.cells.Count);
				if (this.cells[loopIndex].gameObject.activeSelf)
				{
					this.cells[loopIndex].UpdatePosition(num5);
				}
				this.UpdateCellForIndex(this.cells[loopIndex], num6);
				i++;
			}
			loopIndex = this.GetLoopIndex(num3 + i, this.cells.Count);
			while (i < this.cells.Count)
			{
				this.cells[loopIndex].SetVisible(false);
				i++;
				loopIndex = this.GetLoopIndex(num3 + i, this.cells.Count);
			}
		}

		// Token: 0x04006E78 RID: 28280
		[SerializeField]
		[Range(1E-45f, 1f)]
		private float cellInterval;

		// Token: 0x04006E79 RID: 28281
		[SerializeField]
		[Range(0f, 1f)]
		private float cellOffset;

		// Token: 0x04006E7A RID: 28282
		[SerializeField]
		private bool loop;

		// Token: 0x04006E7B RID: 28283
		[SerializeField]
		private GameObject cellBase;

		// Token: 0x04006E7C RID: 28284
		private float currentPosition;

		// Token: 0x04006E7D RID: 28285
		private readonly List<FancyScrollViewCell<TData, TContext>> cells = new List<FancyScrollViewCell<TData, TContext>>();

		// Token: 0x04006E7E RID: 28286
		protected TContext context;

		// Token: 0x04006E7F RID: 28287
		protected List<TData> cellData = new List<TData>();
	}
}
