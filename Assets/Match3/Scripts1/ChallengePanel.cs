using UnityEngine;

// Token: 0x02000905 RID: 2309
namespace Match3.Scripts1
{
	public class ChallengePanel : MonoBehaviour
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x0011355E File Offset: 0x0011195E
		public float AnimationLength
		{
			get
			{
				return this.transitionAnimation["ChallengeContentCenterToLeft"].length;
			}
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x00113578 File Offset: 0x00111978
		public void AnimatePanelDirection(SwipeAnimationDirection swipeAnimationDirection)
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

		// Token: 0x0400605E RID: 24670
		private const string CENTER_TO_LEFT_ANIMATION_NAME = "ChallengeContentCenterToLeft";

		// Token: 0x0400605F RID: 24671
		private const string CENTER_TO_RIGHT_ANIMATION_NAME = "ChallengeContentCenterToRight";

		// Token: 0x04006060 RID: 24672
		private const string LEFT_TO_CENTER_ANIMATION_NAME = "ChallengeContentLeftToCenter";

		// Token: 0x04006061 RID: 24673
		private const string RIGHT_TO_CENTER_ANIMATION_NAME = "ChallengeContentRightToCenter";

		// Token: 0x04006062 RID: 24674
		[SerializeField]
		private Animation transitionAnimation;
	}
}
