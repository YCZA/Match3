using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A6A RID: 2666
	public class CloudsView : MonoBehaviour, ISwipeableView
	{
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003FDA RID: 16346 RVA: 0x00147270 File Offset: 0x00145670
		// (set) Token: 0x06003FDB RID: 16347 RVA: 0x00147278 File Offset: 0x00145678
		public SwipeableViewPosition currentPosition { get; private set; }

		// Token: 0x06003FDC RID: 16348 RVA: 0x00147281 File Offset: 0x00145681
		public void SetupView(SwipeableViewPosition position)
		{
			this.currentPosition = position;
			this.SetAnimatedPosition(position, false);
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x00147294 File Offset: 0x00145694
		public void AnimateView(RelativeDirection direction)
		{
			bool flag = direction != RelativeDirection.Left;
			SwipeableViewPosition viewPosition;
			switch (this.currentPosition)
			{
			case SwipeableViewPosition.left:
				viewPosition = ((!flag) ? SwipeableViewPosition.center : SwipeableViewPosition.left);
				break;
			case SwipeableViewPosition.center:
				viewPosition = ((!flag) ? SwipeableViewPosition.right : SwipeableViewPosition.left);
				break;
			case SwipeableViewPosition.right:
				viewPosition = ((!flag) ? SwipeableViewPosition.right : SwipeableViewPosition.center);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.SetAnimatedPosition(viewPosition, true);
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x00147314 File Offset: 0x00145714
		private void SetAnimatedPosition(SwipeableViewPosition viewPosition, bool animate = true)
		{
			string text = string.Empty;
			if (viewPosition != SwipeableViewPosition.left)
			{
				if (viewPosition != SwipeableViewPosition.center)
				{
					if (viewPosition == SwipeableViewPosition.right)
					{
						text = "2-CloudsCenterToRight";
					}
				}
				else
				{
					text = ((this.currentPosition != SwipeableViewPosition.right) ? "1-CloudsLeftToCenter" : "3-CloudsRightToCenter");
				}
			}
			else
			{
				text = "4-CloudsCenterToLeft";
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

		// Token: 0x0400697A RID: 27002
		private const string LEFT_TO_CENTER = "1-CloudsLeftToCenter";

		// Token: 0x0400697B RID: 27003
		private const string CENTER_TO_RIGHT = "2-CloudsCenterToRight";

		// Token: 0x0400697C RID: 27004
		private const string RIGHT_TO_CENTER = "3-CloudsRightToCenter";

		// Token: 0x0400697D RID: 27005
		private const string CENTER_TO_LEFT = "4-CloudsCenterToLeft";

		// Token: 0x0400697E RID: 27006
		[SerializeField]
		public Animation movementAnimation;
	}
}
