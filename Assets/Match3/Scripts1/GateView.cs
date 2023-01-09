using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020004B0 RID: 1200
namespace Match3.Scripts1
{
	public class GateView : MonoBehaviour
	{
		// Token: 0x060021C1 RID: 8641 RVA: 0x0008EA31 File Offset: 0x0008CE31
		public void SetupView(bool open, Materials rewards)
		{
			this.SetFocusMode(false);
			this.AnimationClipSample((!open) ? this.closeClip : this.openClip);
			this.SetupBubble(rewards);
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x0008EA5E File Offset: 0x0008CE5E
		public void SetFocusMode(bool enabled)
		{
			this.canvasOverride.overrideSorting = enabled;
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x0008EA6C File Offset: 0x0008CE6C
		public IEnumerator PlayCloseAnimation()
		{
			yield return this.AnimationClipPlayWait(this.closeClip);
			yield break;
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0008EA88 File Offset: 0x0008CE88
		public IEnumerator PlayOpen1Animation()
		{
			yield return this.AnimationClipPlayWait(this.closeToCollectClip);
			yield break;
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x0008EAA4 File Offset: 0x0008CEA4
		public IEnumerator PlayOpen2Animation()
		{
			yield return this.AnimationClipPlayWait(this.collectToOpenClip);
			yield break;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0008EAC0 File Offset: 0x0008CEC0
		private void AnimationClipSample(AnimationClip clip)
		{
			AnimationState animationState = this.gateAnimation[clip.name];
			animationState.enabled = true;
			animationState.weight = 1f;
			animationState.normalizedTime = 1f;
			this.gateAnimation.Sample();
			animationState.enabled = false;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0008EB10 File Offset: 0x0008CF10
		private IEnumerator AnimationClipPlayWait(AnimationClip clip)
		{
			if (clip != null)
			{
				this.gateAnimation.Play(clip.name);
				yield return new WaitWhile(() => this.gateAnimation != null && this.gateAnimation.IsPlaying(clip.name));
			}
			yield break;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0008EB32 File Offset: 0x0008CF32
		public void CloseBubble()
		{
			base.StartCoroutine(this.AnimatedBubbleRoutine(false));
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x0008EB42 File Offset: 0x0008CF42
		private void ToggleBubble()
		{
			base.StartCoroutine(this.AnimatedBubbleRoutine(!this.rewardBubble.activeSelf));
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x0008EB60 File Offset: 0x0008CF60
		private void SetupBubble(Materials rewards)
		{
			foreach (MaterialAmountView materialAmountView in this.rewardViews)
			{
				materialAmountView.Hide();
			}
			if (rewards != null)
			{
				for (int i = 0; i < Mathf.Min(rewards.Count, this.rewardViews.Count); i++)
				{
					this.rewardViews[i].Show(rewards[i]);
				}
				if (this.rewardButton != null)
				{
					this.rewardButton.onClick.AddListener(new UnityAction(this.ToggleBubble));
				}
			}
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x0008EC30 File Offset: 0x0008D030
		private IEnumerator AnimatedBubbleRoutine(bool open)
		{
			if (this.animatingBubble)
			{
				yield break;
			}
			if (!open && !this.rewardBubble.activeSelf)
			{
				yield break;
			}
			if (open && this.rewardBubble.activeSelf)
			{
				yield break;
			}
			this.animatingBubble = true;
			string animationName = (!this.rewardBubble.activeSelf) ? "ForeshadowingTreasureDivingExpend" : "ForeshadowingTreasureDivingClose";
			if (open)
			{
				this.rewardBubble.SetActive(true);
			}
			this.rewardBubbleAnimation.Play(animationName);
			yield return new WaitForSeconds(this.rewardBubbleAnimation[animationName].length * 1.25f);
			this.rewardBubble.SetActive(open);
			this.animatingBubble = false;
			yield break;
		}

		// Token: 0x04004D00 RID: 19712
		private const string BUBBLE_OPEN_ANIMATION = "ForeshadowingTreasureDivingExpend";

		// Token: 0x04004D01 RID: 19713
		private const string BUBBLE_CLOSE_ANIMATION = "ForeshadowingTreasureDivingClose";

		// Token: 0x04004D02 RID: 19714
		[SerializeField]
		private Animation gateAnimation;

		// Token: 0x04004D03 RID: 19715
		[SerializeField]
		private AnimationClip openClip;

		// Token: 0x04004D04 RID: 19716
		[SerializeField]
		private AnimationClip closeClip;

		// Token: 0x04004D05 RID: 19717
		[SerializeField]
		private AnimationClip closeToCollectClip;

		// Token: 0x04004D06 RID: 19718
		[SerializeField]
		private AnimationClip collectToOpenClip;

		// Token: 0x04004D07 RID: 19719
		[SerializeField]
		private Canvas canvasOverride;

		// Token: 0x04004D08 RID: 19720
		[Header("Rewards")]
		[SerializeField]
		private Button rewardButton;

		// Token: 0x04004D09 RID: 19721
		[SerializeField]
		private GameObject rewardBubble;

		// Token: 0x04004D0A RID: 19722
		[SerializeField]
		private List<MaterialAmountView> rewardViews;

		// Token: 0x04004D0B RID: 19723
		[SerializeField]
		private Animation rewardBubbleAnimation;

		// Token: 0x04004D0C RID: 19724
		private bool animatingBubble;
	}
}
