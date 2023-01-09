using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BEA RID: 3050
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Horizontal Scroll Snap")]
	public class HorizontalScrollSnap : ScrollSnapBase, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x0600478D RID: 18317 RVA: 0x0016DD74 File Offset: 0x0016C174
		private void Start()
		{
			this._isVertical = false;
			this._childAnchorPoint = new Vector2(0f, 0.5f);
			this._currentPage = this.StartingScreen;
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			this.UpdateLayout();
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0016DDC8 File Offset: 0x0016C1C8
		private void Update()
		{
			if (!this._lerp && this._scroll_rect.velocity == Vector2.zero)
			{
				if (!this._settled && !this._pointerDown && !base.IsRectSettledOnaPage(this._screensContainer.localPosition))
				{
					base.ScrollToClosestElement();
				}
				return;
			}
			if (this._lerp)
			{
				this._screensContainer.localPosition = Vector3.Lerp(this._screensContainer.localPosition, this._lerp_target, this.transitionSpeed * Time.deltaTime);
				if (Vector3.Distance(this._screensContainer.localPosition, this._lerp_target) < 0.1f)
				{
					this._screensContainer.localPosition = this._lerp_target;
					this._lerp = false;
					base.EndScreenChange();
				}
			}
			base.CurrentPage = base.GetPageforPosition(this._screensContainer.localPosition);
			if (!this._pointerDown && ((double)this._scroll_rect.velocity.x > 0.01 || (double)this._scroll_rect.velocity.x < 0.01) && this.IsRectMovingSlowerThanThreshold(0f))
			{
				base.ScrollToClosestElement();
			}
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0016DF20 File Offset: 0x0016C320
		private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
		{
			return (this._scroll_rect.velocity.x > startingSpeed && this._scroll_rect.velocity.x < (float)this.SwipeVelocityThreshold) || (this._scroll_rect.velocity.x < startingSpeed && this._scroll_rect.velocity.x > (float)(-(float)this.SwipeVelocityThreshold));
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0016DFA4 File Offset: 0x0016C3A4
		private void DistributePages()
		{
			this._screens = this._screensContainer.childCount;
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			int num = 0;
			float num2 = 0f;
			float num3 = this._childSize = (float)((int)this.panelDimensions.width) * ((this.PageStep != 0f) ? this.PageStep : 3f);
			for (int i = 0; i < this._screensContainer.transform.childCount; i++)
			{
				RectTransform component = this._screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num2 = (float)(num + (int)((float)i * num3));
				component.sizeDelta = new Vector2(this.panelDimensions.width, this.panelDimensions.height);
				component.anchoredPosition = new Vector2(num2, 0f);
				RectTransform rectTransform = component;
				Vector2 vector = this._childAnchorPoint;
				component.pivot = vector;
				vector = vector;
				component.anchorMax = vector;
				rectTransform.anchorMin = vector;
			}
			float x = num2 + (float)(num * -1);
			this._screensContainer.offsetMax = new Vector2(x, 0f);
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0016E0DF File Offset: 0x0016C4DF
		public void AddChild(GameObject GO)
		{
			this.AddChild(GO, false);
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0016E0EC File Offset: 0x0016C4EC
		public void AddChild(GameObject GO, bool WorldPositionStays)
		{
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			GO.transform.SetParent(this._screensContainer, WorldPositionStays);
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x0016E143 File Offset: 0x0016C543
		public void RemoveChild(int index, out GameObject ChildRemoved)
		{
			this.RemoveChild(index, false, out ChildRemoved);
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0016E150 File Offset: 0x0016C550
		public void RemoveChild(int index, bool WorldPositionStays, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
			if (index < 0 || index > this._screensContainer.childCount)
			{
				return;
			}
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			Transform child = this._screensContainer.transform.GetChild(index);
			child.SetParent(null, WorldPositionStays);
			ChildRemoved = child.gameObject;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			if (this._currentPage > this._screens - 1)
			{
				base.CurrentPage = this._screens - 1;
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0016E1F4 File Offset: 0x0016C5F4
		public void RemoveAllChildren(out GameObject[] ChildrenRemoved)
		{
			this.RemoveAllChildren(false, out ChildrenRemoved);
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0016E200 File Offset: 0x0016C600
		public void RemoveAllChildren(bool WorldPositionStays, out GameObject[] ChildrenRemoved)
		{
			int childCount = this._screensContainer.childCount;
			ChildrenRemoved = new GameObject[childCount];
			for (int i = childCount - 1; i >= 0; i--)
			{
				ChildrenRemoved[i] = this._screensContainer.GetChild(i).gameObject;
				ChildrenRemoved[i].transform.SetParent(null, WorldPositionStays);
			}
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			base.CurrentPage = 0;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0016E294 File Offset: 0x0016C694
		private void SetScrollContainerPosition()
		{
			this._scrollStartPosition = this._screensContainer.localPosition.x;
			this._scroll_rect.horizontalNormalizedPosition = (float)this._currentPage / (float)(this._screens - 1);
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0016E2E2 File Offset: 0x0016C6E2
		public void UpdateLayout()
		{
			this._lerp = false;
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0016E319 File Offset: 0x0016C719
		private void OnRectTransformDimensionsChange()
		{
			if (this._childAnchorPoint != Vector2.zero)
			{
				this.UpdateLayout();
			}
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0016E338 File Offset: 0x0016C738
		private void OnEnable()
		{
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			if (this.JumpOnEnable || !this.RestartOnEnable)
			{
				this.SetScrollContainerPosition();
			}
			if (this.RestartOnEnable)
			{
				base.GoToScreen(this.StartingScreen);
			}
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0016E39C File Offset: 0x0016C79C
		public void OnEndDrag(PointerEventData eventData)
		{
			this._pointerDown = false;
			if (this._scroll_rect.horizontal)
			{
				float num = Vector3.Distance(this._startPosition, this._screensContainer.localPosition);
				if (this.UseFastSwipe && num < this.panelDimensions.width && num >= (float)this.FastSwipeThreshold)
				{
					this._scroll_rect.velocity = Vector3.zero;
					if (this._startPosition.x - this._screensContainer.localPosition.x > 0f)
					{
						base.NextScreen();
					}
					else
					{
						base.PreviousScreen();
					}
				}
			}
		}
	}
}
