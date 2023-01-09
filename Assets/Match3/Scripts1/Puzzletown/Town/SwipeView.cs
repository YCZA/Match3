using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x02000A24 RID: 2596
	public class SwipeView : MonoBehaviour
	{
		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x0013BFC8 File Offset: 0x0013A3C8
		public float AnimationLength
		{
			get
			{
				return this.transitionAnimation["ChallengeContentCenterToLeft"].length;
			}
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0013BFE0 File Offset: 0x0013A3E0
		public void AnimateViewDirection(SwipeAnimationDirection swipeAnimationDirection)
		{
			switch (swipeAnimationDirection)
			{
			case SwipeAnimationDirection.CenterToLeft:
				this.transitionAnimation["ChallengeContentCenterToLeft"].time = 0f;
				this.transitionAnimation.Play("ChallengeContentCenterToLeft");
				break;
			case SwipeAnimationDirection.CenterToRight:
				this.transitionAnimation["ChallengeContentCenterToRight"].time = 0f;
				this.transitionAnimation.Play("ChallengeContentCenterToRight");
				break;
			case SwipeAnimationDirection.LeftToCenter:
				this.transitionAnimation["ChallengeContentLeftToCenter"].time = 0f;
				this.transitionAnimation.Play("ChallengeContentLeftToCenter");
				break;
			case SwipeAnimationDirection.RightToCenter:
				this.transitionAnimation["ChallengeContentRightToCenter"].time = 0f;
				this.transitionAnimation.Play("ChallengeContentRightToCenter");
				break;
			}
		}

		// Token: 0x0400675C RID: 26460
		private const string CENTER_TO_LEFT_ANIMATION_NAME = "ChallengeContentCenterToLeft";

		// Token: 0x0400675D RID: 26461
		private const string CENTER_TO_RIGHT_ANIMATION_NAME = "ChallengeContentCenterToRight";

		// Token: 0x0400675E RID: 26462
		private const string LEFT_TO_CENTER_ANIMATION_NAME = "ChallengeContentLeftToCenter";

		// Token: 0x0400675F RID: 26463
		private const string RIGHT_TO_CENTER_ANIMATION_NAME = "ChallengeContentRightToCenter";

		// Token: 0x04006760 RID: 26464
		[SerializeField]
		private Animation transitionAnimation;
	}
}
