using System;
using System.Collections;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B44 RID: 2884
	[RequireComponent(typeof(ScrollRect))]
	public class TableViewSnapper : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060043A2 RID: 17314 RVA: 0x00159768 File Offset: 0x00157B68
		public ScrollRect scrollRect
		{
			get
			{
				if (!this._scrollRect)
				{
					this._scrollRect = base.GetComponent<ScrollRect>();
				}
				return this._scrollRect;
			}
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0015978C File Offset: 0x00157B8C
		public void Snap(float position)
		{
			base.StartCoroutine(this.SnapTo(this.GetInternalSnapPosition(position)));
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x001597A2 File Offset: 0x00157BA2
		public void ScrollTo(float position, float time, Action<IEnumerator> startCoroutine)
		{
			startCoroutine(this.ScrollToRoutine(this.GetInternalSnapPosition(position), time));
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x001597B8 File Offset: 0x00157BB8
		public void ScrollTo(float position, float time)
		{
			base.StartCoroutine(this.ScrollToRoutine(this.GetInternalSnapPosition(position), time));
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x001597D0 File Offset: 0x00157BD0
		public float GetInternalSnapPosition(float position)
		{
			float height = this.scrollRect.content.rect.height;
			float height2 = this.scrollRect.viewport.rect.height;
			float num = height2 / height;
			if (num >= 1f)
			{
				return 0f;
			}
			LayoutGroup component = this.scrollRect.content.GetComponent<LayoutGroup>();
			if (component && !this.ignorePadding)
			{
				RectOffset padding = component.padding;
				position = (position * (height - (float)padding.vertical) + (float)padding.bottom) / height;
			}
			return Mathf.Clamp01((position - num * 0.5f) / (1f - num));
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0015988C File Offset: 0x00157C8C
		private IEnumerator SnapTo(float position)
		{
			yield return new WaitForEndOfFrame();
			this.scrollRect.verticalNormalizedPosition = position;
			yield return null;
			yield break;
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x001598B0 File Offset: 0x00157CB0
		private IEnumerator ScrollToRoutine(float targetPosition, float time)
		{
			float startTime = Time.timeSinceLevelLoad;
			float ratio = 0f;
			float startPosition = this.scrollRect.verticalNormalizedPosition;
			while (ratio < 1f)
			{
				float elapsedTime = Time.timeSinceLevelLoad - startTime;
				ratio = elapsedTime / time;
				this.scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, ratio);
				yield return null;
			}
			this.scrollRect.verticalNormalizedPosition = targetPosition;
			yield break;
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x001598D9 File Offset: 0x00157CD9
		public void OnBeginDrag(PointerEventData evt)
		{
			this.onBeginDrag.Dispatch(evt);
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x001598E7 File Offset: 0x00157CE7
		public void OnEndDrag(PointerEventData evt)
		{
			this.onEndDrag.Dispatch(evt);
		}

		// Token: 0x04006C0C RID: 27660
		private ScrollRect _scrollRect;

		// Token: 0x04006C0D RID: 27661
		public bool ignorePadding;

		// Token: 0x04006C0E RID: 27662
		public readonly Signal<PointerEventData> onEndDrag = new Signal<PointerEventData>();

		// Token: 0x04006C0F RID: 27663
		public readonly Signal<PointerEventData> onBeginDrag = new Signal<PointerEventData>();
	}
}
