using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A6C RID: 2668
	public class IslandView : MonoBehaviour, ISwipeableView, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x001473CD File Offset: 0x001457CD
		public float AnimationTime
		{
			get
			{
				return this.movementAnimation["2-LeftEdgeToCenter"].length;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x001473E4 File Offset: 0x001457E4
		// (set) Token: 0x06003FE2 RID: 16354 RVA: 0x001473EC File Offset: 0x001457EC
		public SwipeableViewPosition currentPosition { get; private set; }

		// Token: 0x06003FE3 RID: 16355 RVA: 0x001473F8 File Offset: 0x001457F8
		public void SetupView(int islandId, Sprite sprite, SwipeableViewPosition position, bool lockStatus, bool bundleAvailable, DateTime? unlockDate, SwipePanel swipePanel, WorldMapRoot.SelectionDelegate selectionDelegate)
		{
			this.islandId = islandId;
			this.selectionDelegate = selectionDelegate;
			this.image.sprite = sprite;
			this.bundleAvailable = bundleAvailable;
			this.swipePanel = swipePanel;
			this.SetAnimatedPosition(position, false);
			this.SetLockState(lockStatus);
			this.SetUnlockDate(unlockDate);
			this.initialized = true;
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0014744F File Offset: 0x0014584F
		public void RefreshBundleAvailability(bool bundleAvailable)
		{
			if (this.initialized)
			{
				this.bundleAvailable = bundleAvailable;
				this.SetLockState(this.isLocked);
			}
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0014746F File Offset: 0x0014586F
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.dragging)
			{
				this.selectionDelegate(this.islandId);
			}
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0014748D File Offset: 0x0014588D
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.dragging = true;
			this.swipePanel.OnBeginDrag(eventData);
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x001474A2 File Offset: 0x001458A2
		public void OnDrag(PointerEventData eventData)
		{
			this.swipePanel.OnDrag(eventData);
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x001474B0 File Offset: 0x001458B0
		public void OnEndDrag(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x001474BC File Offset: 0x001458BC
		public void AnimateView(RelativeDirection direction)
		{
			bool flag = direction != RelativeDirection.Left;
			SwipeableViewPosition viewPosition = SwipeableViewPosition.offscreenLeft;
			switch (this.currentPosition)
			{
			case SwipeableViewPosition.offscreenLeft:
				viewPosition = ((!flag) ? SwipeableViewPosition.left : SwipeableViewPosition.offscreenLeft);
				break;
			case SwipeableViewPosition.left:
				viewPosition = ((!flag) ? SwipeableViewPosition.center : SwipeableViewPosition.offscreenLeft);
				break;
			case SwipeableViewPosition.center:
				viewPosition = ((!flag) ? SwipeableViewPosition.right : SwipeableViewPosition.left);
				break;
			case SwipeableViewPosition.right:
				viewPosition = ((!flag) ? SwipeableViewPosition.offscreenRight : SwipeableViewPosition.center);
				break;
			case SwipeableViewPosition.offscreenRight:
				viewPosition = ((!flag) ? SwipeableViewPosition.offscreenRight : SwipeableViewPosition.right);
				break;
			}
			this.SetAnimatedPosition(viewPosition, true);
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00147560 File Offset: 0x00145960
		private void SetAnimatedPosition(SwipeableViewPosition viewPosition, bool animate = true)
		{
			string text = string.Empty;
			switch (viewPosition)
			{
			case SwipeableViewPosition.offscreenLeft:
				text = "8-LeftEdgeToLeftOut";
				break;
			case SwipeableViewPosition.left:
				text = ((this.currentPosition != SwipeableViewPosition.center) ? "1-LeftOutToLeftEdge" : "7-CenterToLeftEdge");
				break;
			case SwipeableViewPosition.center:
				text = ((this.currentPosition != SwipeableViewPosition.right) ? "2-LeftEdgeToCenter" : "6-RightEdgeToCenter");
				break;
			case SwipeableViewPosition.right:
				text = ((this.currentPosition != SwipeableViewPosition.center) ? "5-RightOutToRightEdge" : "3-CenterToRightEdge");
				break;
			case SwipeableViewPosition.offscreenRight:
				text = "4-RightEdgeToRightOut";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (animate)
				{
					this.movementAnimation.Play(text);
				}
				else
				{
					this.movementAnimation[text].time = 1f;
					this.movementAnimation.Play(text);
				}
			}
			this.currentPosition = viewPosition;
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x00147658 File Offset: 0x00145A58
		private void SetLockState(bool isLocked)
		{
			this.isLocked = isLocked;
			if (this.lockBubble != null)
			{
				this.lockBubble.SetActive(isLocked);
			}
			if (this.spinner != null)
			{
				this.spinner.SetActive(!this.bundleAvailable && !isLocked);
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x001476B8 File Offset: 0x00145AB8
		private void SetUnlockDate(DateTime? unlockDate)
		{
			bool flag = unlockDate != null;
			this.lockBubble.SetActive(!flag && this.isLocked);
			this.countdownTimer.gameObject.SetActive(flag);
			if (flag)
			{
				this.countdownTimer.SetTargetTime(unlockDate.Value, false, null);
			}
		}

		// Token: 0x04006984 RID: 27012
		private const string LEFT_OUT_TO_LEFT_EDGE = "1-LeftOutToLeftEdge";

		// Token: 0x04006985 RID: 27013
		private const string LEFT_EDGE_TO_CENTER = "2-LeftEdgeToCenter";

		// Token: 0x04006986 RID: 27014
		private const string CENTER_TO_RIGHT_EDGE = "3-CenterToRightEdge";

		// Token: 0x04006987 RID: 27015
		private const string RIGHT_EDGE_TO_RIGHT_OUT = "4-RightEdgeToRightOut";

		// Token: 0x04006988 RID: 27016
		private const string RIGHT_OUT_TO_RIGHT_EDGE = "5-RightOutToRightEdge";

		// Token: 0x04006989 RID: 27017
		private const string RIGHT_EDGE_TO_CENTER = "6-RightEdgeToCenter";

		// Token: 0x0400698A RID: 27018
		private const string CENTER_TO_LEFT_EDGE = "7-CenterToLeftEdge";

		// Token: 0x0400698B RID: 27019
		private const string LEFT_EDGE_TO_LEFT_OUT = "8-LeftEdgeToLeftOut";

		// Token: 0x0400698C RID: 27020
		public int islandId;

		// Token: 0x0400698D RID: 27021
		[SerializeField]
		private Image image;

		// Token: 0x0400698E RID: 27022
		[SerializeField]
		private Animation movementAnimation;

		// Token: 0x0400698F RID: 27023
		[SerializeField]
		private GameObject lockBubble;

		// Token: 0x04006990 RID: 27024
		[SerializeField]
		private GameObject spinner;

		// Token: 0x04006991 RID: 27025
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x04006992 RID: 27026
		private WorldMapRoot.SelectionDelegate selectionDelegate;

		// Token: 0x04006993 RID: 27027
		private SwipePanel swipePanel;

		// Token: 0x04006994 RID: 27028
		private bool dragging;

		// Token: 0x04006995 RID: 27029
		private bool isLocked;

		// Token: 0x04006996 RID: 27030
		private bool bundleAvailable;

		// Token: 0x04006997 RID: 27031
		private bool initialized;
	}
}
