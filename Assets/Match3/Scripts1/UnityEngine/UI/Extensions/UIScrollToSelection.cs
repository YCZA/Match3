using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C39 RID: 3129
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/UIScrollToSelection")]
	public class UIScrollToSelection : MonoBehaviour
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x00179728 File Offset: 0x00177B28
		protected RectTransform LayoutListGroup
		{
			get
			{
				return (!(this.TargetScrollRect != null)) ? null : this.TargetScrollRect.content;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060049C2 RID: 18882 RVA: 0x0017974C File Offset: 0x00177B4C
		protected UIScrollToSelection.ScrollType ScrollDirection
		{
			get
			{
				return this.scrollDirection;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x00179754 File Offset: 0x00177B54
		protected float ScrollSpeed
		{
			get
			{
				return this.scrollSpeed;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x0017975C File Offset: 0x00177B5C
		protected bool CancelScrollOnInput
		{
			get
			{
				return this.cancelScrollOnInput;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x00179764 File Offset: 0x00177B64
		protected List<KeyCode> CancelScrollKeycodes
		{
			get
			{
				return this.cancelScrollKeycodes;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x0017976C File Offset: 0x00177B6C
		// (set) Token: 0x060049C7 RID: 18887 RVA: 0x00179774 File Offset: 0x00177B74
		protected RectTransform ScrollWindow { get; set; }

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x0017977D File Offset: 0x00177B7D
		// (set) Token: 0x060049C9 RID: 18889 RVA: 0x00179785 File Offset: 0x00177B85
		protected ScrollRect TargetScrollRect { get; set; }

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x0017978E File Offset: 0x00177B8E
		protected EventSystem CurrentEventSystem
		{
			get
			{
				return EventSystem.current;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060049CB RID: 18891 RVA: 0x00179795 File Offset: 0x00177B95
		// (set) Token: 0x060049CC RID: 18892 RVA: 0x0017979D File Offset: 0x00177B9D
		protected GameObject LastCheckedGameObject { get; set; }

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x001797A6 File Offset: 0x00177BA6
		protected GameObject CurrentSelectedGameObject
		{
			get
			{
				return EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060049CE RID: 18894 RVA: 0x001797B2 File Offset: 0x00177BB2
		// (set) Token: 0x060049CF RID: 18895 RVA: 0x001797BA File Offset: 0x00177BBA
		protected RectTransform CurrentTargetRectTransform { get; set; }

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x001797C3 File Offset: 0x00177BC3
		// (set) Token: 0x060049D1 RID: 18897 RVA: 0x001797CB File Offset: 0x00177BCB
		protected bool IsManualScrollingAvailable { get; set; }

		// Token: 0x060049D2 RID: 18898 RVA: 0x001797D4 File Offset: 0x00177BD4
		protected virtual void Awake()
		{
			this.TargetScrollRect = base.GetComponent<ScrollRect>();
			this.ScrollWindow = this.TargetScrollRect.GetComponent<RectTransform>();
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x001797F3 File Offset: 0x00177BF3
		protected virtual void Start()
		{
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x001797F5 File Offset: 0x00177BF5
		protected virtual void Update()
		{
			this.UpdateReferences();
			this.CheckIfScrollingShouldBeLocked();
			this.ScrollRectToLevelSelection();
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x0017980C File Offset: 0x00177C0C
		private void UpdateReferences()
		{
			if (this.CurrentSelectedGameObject != this.LastCheckedGameObject)
			{
				this.CurrentTargetRectTransform = ((!(this.CurrentSelectedGameObject != null)) ? null : this.CurrentSelectedGameObject.GetComponent<RectTransform>());
				if (this.CurrentSelectedGameObject != null && this.CurrentSelectedGameObject.transform.parent == this.LayoutListGroup.transform)
				{
					this.IsManualScrollingAvailable = false;
				}
			}
			this.LastCheckedGameObject = this.CurrentSelectedGameObject;
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x001798A0 File Offset: 0x00177CA0
		private void CheckIfScrollingShouldBeLocked()
		{
			if (!this.CancelScrollOnInput || this.IsManualScrollingAvailable)
			{
				return;
			}
			for (int i = 0; i < this.CancelScrollKeycodes.Count; i++)
			{
				if (global::UnityEngine.Input.GetKeyDown(this.CancelScrollKeycodes[i]))
				{
					this.IsManualScrollingAvailable = true;
					break;
				}
			}
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x00179904 File Offset: 0x00177D04
		private void ScrollRectToLevelSelection()
		{
			if (this.TargetScrollRect == null || this.LayoutListGroup == null || this.ScrollWindow == null || this.IsManualScrollingAvailable)
			{
				return;
			}
			RectTransform currentTargetRectTransform = this.CurrentTargetRectTransform;
			if (currentTargetRectTransform == null || currentTargetRectTransform.transform.parent != this.LayoutListGroup.transform)
			{
				return;
			}
			UIScrollToSelection.ScrollType scrollType = this.ScrollDirection;
			if (scrollType != UIScrollToSelection.ScrollType.VERTICAL)
			{
				if (scrollType != UIScrollToSelection.ScrollType.HORIZONTAL)
				{
					if (scrollType == UIScrollToSelection.ScrollType.BOTH)
					{
						this.UpdateVerticalScrollPosition(currentTargetRectTransform);
						this.UpdateHorizontalScrollPosition(currentTargetRectTransform);
					}
				}
				else
				{
					this.UpdateHorizontalScrollPosition(currentTargetRectTransform);
				}
			}
			else
			{
				this.UpdateVerticalScrollPosition(currentTargetRectTransform);
			}
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x001799D4 File Offset: 0x00177DD4
		private void UpdateVerticalScrollPosition(RectTransform selection)
		{
			float position = -selection.anchoredPosition.y - selection.rect.height * (1f - selection.pivot.y);
			float height = selection.rect.height;
			float height2 = this.ScrollWindow.rect.height;
			float y = this.LayoutListGroup.anchoredPosition.y;
			float scrollOffset = this.GetScrollOffset(position, y, height, height2);
			this.TargetScrollRect.verticalNormalizedPosition += scrollOffset / this.LayoutListGroup.rect.height * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00179A98 File Offset: 0x00177E98
		private void UpdateHorizontalScrollPosition(RectTransform selection)
		{
			float position = -selection.anchoredPosition.x - selection.rect.width * (1f - selection.pivot.x);
			float width = selection.rect.width;
			float width2 = this.ScrollWindow.rect.width;
			float listAnchorPosition = -this.LayoutListGroup.anchoredPosition.x;
			float num = -this.GetScrollOffset(position, listAnchorPosition, width, width2);
			this.TargetScrollRect.horizontalNormalizedPosition += num / this.LayoutListGroup.rect.width * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x00179B5D File Offset: 0x00177F5D
		private float GetScrollOffset(float position, float listAnchorPosition, float targetLength, float maskLength)
		{
			if (position < listAnchorPosition + targetLength / 2f)
			{
				return listAnchorPosition + maskLength - (position - targetLength);
			}
			if (position + targetLength > listAnchorPosition + maskLength)
			{
				return listAnchorPosition + maskLength - (position + targetLength);
			}
			return 0f;
		}

		// Token: 0x04007018 RID: 28696
		[Header("[ Settings ]")]
		[SerializeField]
		private UIScrollToSelection.ScrollType scrollDirection;

		// Token: 0x04007019 RID: 28697
		[SerializeField]
		private float scrollSpeed = 10f;

		// Token: 0x0400701A RID: 28698
		[Header("[ Input ]")]
		[SerializeField]
		private bool cancelScrollOnInput;

		// Token: 0x0400701B RID: 28699
		[SerializeField]
		private List<KeyCode> cancelScrollKeycodes = new List<KeyCode>();

		// Token: 0x02000C3A RID: 3130
		public enum ScrollType
		{
			// Token: 0x04007022 RID: 28706
			VERTICAL,
			// Token: 0x04007023 RID: 28707
			HORIZONTAL,
			// Token: 0x04007024 RID: 28708
			BOTH
		}
	}
}
