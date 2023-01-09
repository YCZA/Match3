using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BFF RID: 3071
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroll Snap")]
	public class VerticalScrollSnap : ScrollSnapBase, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06004830 RID: 18480 RVA: 0x001707F0 File Offset: 0x0016EBF0
		private void Start()
		{
			this._isVertical = true;
			this._childAnchorPoint = new Vector2(0.5f, 0f);
			this._currentPage = this.StartingScreen;
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			this.UpdateLayout();
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x00170844 File Offset: 0x0016EC44
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
			if (!this._pointerDown && ((double)this._scroll_rect.velocity.y > 0.01 || (double)this._scroll_rect.velocity.y < -0.01) && this.IsRectMovingSlowerThanThreshold(0f))
			{
				base.ScrollToClosestElement();
			}
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x0017099C File Offset: 0x0016ED9C
		private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
		{
			return (this._scroll_rect.velocity.y > startingSpeed && this._scroll_rect.velocity.y < (float)this.SwipeVelocityThreshold) || (this._scroll_rect.velocity.y < startingSpeed && this._scroll_rect.velocity.y > (float)(-(float)this.SwipeVelocityThreshold));
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x00170A20 File Offset: 0x0016EE20
		public void DistributePages()
		{
			this._screens = this._screensContainer.childCount;
			this._scroll_rect.verticalNormalizedPosition = 0f;
			float num = 0f;
			Rect rect = base.gameObject.GetComponent<RectTransform>().rect;
			float num2 = 0f;
			float num3 = this._childSize = (float)((int)rect.height) * ((this.PageStep != 0f) ? this.PageStep : 3f);
			for (int i = 0; i < this._screensContainer.transform.childCount; i++)
			{
				RectTransform component = this._screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num2 = num + (float)i * num3;
				component.sizeDelta = new Vector2(rect.width, rect.height);
				component.anchoredPosition = new Vector2(0f, num2);
				RectTransform rectTransform = component;
				Vector2 vector = this._childAnchorPoint;
				component.pivot = vector;
				vector = vector;
				component.anchorMax = vector;
				rectTransform.anchorMin = vector;
			}
			float y = num2 + num * -1f;
			this._screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, y);
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x00170B6C File Offset: 0x0016EF6C
		public void AddChild(GameObject GO)
		{
			this.AddChild(GO, false);
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x00170B78 File Offset: 0x0016EF78
		public void AddChild(GameObject GO, bool WorldPositionStays)
		{
			this._scroll_rect.verticalNormalizedPosition = 0f;
			GO.transform.SetParent(this._screensContainer, WorldPositionStays);
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x00170BCF File Offset: 0x0016EFCF
		public void RemoveChild(int index, out GameObject ChildRemoved)
		{
			this.RemoveChild(index, false, out ChildRemoved);
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x00170BDC File Offset: 0x0016EFDC
		public void RemoveChild(int index, bool WorldPositionStays, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
			if (index < 0 || index > this._screensContainer.childCount)
			{
				return;
			}
			this._scroll_rect.verticalNormalizedPosition = 0f;
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

		// Token: 0x06004838 RID: 18488 RVA: 0x00170C80 File Offset: 0x0016F080
		public void RemoveAllChildren(out GameObject[] ChildrenRemoved)
		{
			this.RemoveAllChildren(false, out ChildrenRemoved);
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00170C8C File Offset: 0x0016F08C
		public void RemoveAllChildren(bool WorldPositionStays, out GameObject[] ChildrenRemoved)
		{
			int childCount = this._screensContainer.childCount;
			ChildrenRemoved = new GameObject[childCount];
			for (int i = childCount - 1; i >= 0; i--)
			{
				ChildrenRemoved[i] = this._screensContainer.GetChild(i).gameObject;
				ChildrenRemoved[i].transform.SetParent(null, WorldPositionStays);
			}
			this._scroll_rect.verticalNormalizedPosition = 0f;
			base.CurrentPage = 0;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00170D20 File Offset: 0x0016F120
		private void SetScrollContainerPosition()
		{
			this._scrollStartPosition = this._screensContainer.localPosition.y;
			this._scroll_rect.verticalNormalizedPosition = (float)this._currentPage / (float)(this._screens - 1);
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x00170D6E File Offset: 0x0016F16E
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

		// Token: 0x0600483C RID: 18492 RVA: 0x00170DA5 File Offset: 0x0016F1A5
		private void OnRectTransformDimensionsChange()
		{
			if (this._childAnchorPoint != Vector2.zero)
			{
				this.UpdateLayout();
			}
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x00170DC4 File Offset: 0x0016F1C4
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

		// Token: 0x0600483E RID: 18494 RVA: 0x00170E28 File Offset: 0x0016F228
		public void OnEndDrag(PointerEventData eventData)
		{
			this._pointerDown = false;
			if (this._scroll_rect.vertical)
			{
				float num = Vector3.Distance(this._startPosition, this._screensContainer.localPosition);
				if (this.UseFastSwipe && num < this.panelDimensions.height + (float)this.FastSwipeThreshold && num >= 1f)
				{
					this._scroll_rect.velocity = Vector3.zero;
					if (this._startPosition.y - this._screensContainer.localPosition.y > 0f)
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
