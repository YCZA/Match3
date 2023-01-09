using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000717 RID: 1815
	public class BoostMaskFader : ABoostMaskListener
	{
		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x000D0851 File Offset: 0x000CEC51
		public float Opacity
		{
			get
			{
				return (!this.useCustomOpacity) ? ABoostMaskListener.BOOST_OVERLAY_OPACITY : this.customOpacity;
			}
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000D0870 File Offset: 0x000CEC70
		public override void HandleBoostOverlayStateChanged(BoostOverlayState newState)
		{
			if (this.targetImage != null)
			{
				float target = (!newState.isOn) ? 0f : this.Opacity;
				float duration = (!newState.shouldChangeInstantly) ? BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS : 0f;
				if (newState.isOn)
				{
					this.targetImage.gameObject.SetActive(true);
				}
				bool flag = this.tweener == null || !this.tweener.IsPlaying() || this.previousTarget != newState.isOn;
				if (flag)
				{
					if (this.tweener != null)
					{
						this.tweener.Kill(false);
					}
					this.tweener = this.StartNewTween(target, duration, newState.isOn);
					this.previousTarget = newState.isOn;
				}
			}
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000D0958 File Offset: 0x000CED58
		private Tweener StartNewTween(float target, float duration, bool isOn)
		{
			if (!this.targetImage.enabled)
			{
				this.targetImage.enabled = true;
				if (this.startAlphaSource != null)
				{
					Color color = this.targetImage.color;
					this.targetImage.color = new Color(color.r, color.g, color.b, this.startAlphaSource.color.a);
					duration -= this.boostOverlayController.TickRoutineElapsedTime;
				}
			}
			return this.targetImage.DOFade(target, duration).SetRecyclable(true).OnKill(delegate
			{
				this.tweener = null;
			}).OnComplete(delegate
			{
				if (this.targetImage.gameObject != null)
				{
					this.targetImage.gameObject.SetActive(isOn);
				}
			});
		}

		// Token: 0x04005677 RID: 22135
		public Image targetImage;

		// Token: 0x04005678 RID: 22136
		public Image startAlphaSource;

		// Token: 0x04005679 RID: 22137
		public bool useCustomOpacity;

		// Token: 0x0400567A RID: 22138
		[Range(0f, 1f)]
		public float customOpacity;

		// Token: 0x0400567B RID: 22139
		private Tweener tweener;

		// Token: 0x0400567C RID: 22140
		private bool previousTarget;
	}
}
