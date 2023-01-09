using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x020004AC RID: 1196
namespace Match3.Scripts1
{
	public class ChestView : MonoBehaviour
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x0008CF79 File Offset: 0x0008B379
		public float AnimationTime
		{
			get
			{
				return this.animationTime;
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0008CF84 File Offset: 0x0008B384
		public void SetupView(bool chestOpen, GameStateService gameStateService)
		{
			this.chestOpen = chestOpen;
			this.openChestView.SetActive(chestOpen);
			this.closedChestView.SetActive(!chestOpen);
			this.rewardBubble.SetActive(!chestOpen);
			if (gameStateService.DiveForTreasure.IsRewardedLevel(this.level))
			{
				Materials rewards = gameStateService.DiveForTreasure.GetRewards(this.level);
				if (rewards != null)
				{
					for (int i = 0; i < rewards.Count; i++)
					{
						this.rewardViews[i].Show(rewards[i]);
					}
				}
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0008D020 File Offset: 0x0008B420
		public void PlayOpenAnimation()
		{
			this.rewardBubble.SetActive(false);
			this.chestOpen = true;
			if (this.openAnimation["DiveForTreasureLevel3Transition"])
			{
				this.openAnimation.Play("DiveForTreasureLevel3Transition");
				this.animationTime = this.openAnimation["DiveForTreasureLevel3Transition"].length;
			}
			if (this.openAnimation["DiveForTreasureLevel8Transition"])
			{
				this.openAnimation.Play("DiveForTreasureLevel8Transition");
				this.animationTime = this.openAnimation["DiveForTreasureLevel8Transition"].length;
			}
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x0008D0CC File Offset: 0x0008B4CC
		public void CloseBubble()
		{
			base.StartCoroutine(this.AnimatedBubbleRoutine(false));
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0008D0DC File Offset: 0x0008B4DC
		public void ShowBubble()
		{
			base.StartCoroutine(this.AnimatedBubbleRoutine(true));
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0008D0EC File Offset: 0x0008B4EC
		private void OnMouseDown()
		{
			if (!this.chestOpen)
			{
				base.StartCoroutine(this.AnimatedBubbleRoutine(!this.rewardBubble.activeSelf));
			}
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0008D114 File Offset: 0x0008B514
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
			this.bubbleAnimation.Play(animationName);
			yield return new WaitForSeconds(this.bubbleAnimation[animationName].length * 1.25f);
			this.rewardBubble.SetActive(open);
			this.animatingBubble = false;
			yield break;
		}

		// Token: 0x04004CCA RID: 19658
		private const string CHEST_ONE_ANIMATION_NAME = "DiveForTreasureLevel3Transition";

		// Token: 0x04004CCB RID: 19659
		private const string CHEST_TWO_ANIMATION_NAME = "DiveForTreasureLevel8Transition";

		// Token: 0x04004CCC RID: 19660
		private const string BUBBLE_OPEN_ANIMATION = "ForeshadowingTreasureDivingExpend";

		// Token: 0x04004CCD RID: 19661
		private const string BUBBLE_CLOSE_ANIMATION = "ForeshadowingTreasureDivingClose";

		// Token: 0x04004CCE RID: 19662
		[SerializeField]
		private GameObject openChestView;

		// Token: 0x04004CCF RID: 19663
		[SerializeField]
		private GameObject closedChestView;

		// Token: 0x04004CD0 RID: 19664
		[SerializeField]
		private Animation openAnimation;

		// Token: 0x04004CD1 RID: 19665
		[SerializeField]
		private GameObject rewardBubble;

		// Token: 0x04004CD2 RID: 19666
		[SerializeField]
		private int level;

		// Token: 0x04004CD3 RID: 19667
		[SerializeField]
		private List<MaterialAmountView> rewardViews;

		// Token: 0x04004CD4 RID: 19668
		[SerializeField]
		private Animation bubbleAnimation;

		// Token: 0x04004CD5 RID: 19669
		private float animationTime;

		// Token: 0x04004CD6 RID: 19670
		private bool animatingBubble;

		// Token: 0x04004CD7 RID: 19671
		private bool chestOpen;
	}
}
