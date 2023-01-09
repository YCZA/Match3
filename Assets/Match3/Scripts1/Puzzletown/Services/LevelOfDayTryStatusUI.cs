using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007D3 RID: 2003
	public class LevelOfDayTryStatusUI : MonoBehaviour
	{
		// Token: 0x0600316C RID: 12652 RVA: 0x000E8767 File Offset: 0x000E6B67
		protected void OnValidate()
		{
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x000E876C File Offset: 0x000E6B6C
		public void Init(int triesSoFar, bool showAnimated, float animationDelaySeconds)
		{
			int num = (!showAnimated) ? triesSoFar : (triesSoFar - 1);
			for (int i = 0; i < this.triesAvailable.Length; i++)
			{
				bool flag = i + 1 <= num;
				this.triesFailed[i].SetActive(flag);
				this.triesAvailable[i].SetActive(!flag);
			}
			if (triesSoFar > 0 && showAnimated)
			{
				WooroutineRunner.StartCoroutine(this.PlayAnimationAfterDelayRoutine(triesSoFar, animationDelaySeconds), null);
			}
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x000E87E8 File Offset: 0x000E6BE8
		protected IEnumerator PlayAnimationAfterDelayRoutine(int triesSoFar, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.PlayAnimation(triesSoFar);
			yield break;
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x000E8814 File Offset: 0x000E6C14
		public void PlayAnimation(int triesSoFar)
		{
			if (this.animationClips != null && this.animation != null)
			{
				int num = Mathf.Clamp(triesSoFar - 1, 0, this.animationClips.Length - 1);
				AnimationClip animationClip = this.animationClips[num];
				if (animationClip != null)
				{
					this.animation.Play(animationClip.name, PlayMode.StopSameLayer);
				}
			}
		}

		// Token: 0x04005A02 RID: 23042
		public GameObject[] triesAvailable;

		// Token: 0x04005A03 RID: 23043
		public GameObject[] triesFailed;

		// Token: 0x04005A04 RID: 23044
		public Animation animation;

		// Token: 0x04005A05 RID: 23045
		public AnimationClip[] animationClips;
	}
}
