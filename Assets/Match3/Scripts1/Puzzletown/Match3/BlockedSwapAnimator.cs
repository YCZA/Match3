using System.Collections.Generic;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000650 RID: 1616
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/BlockedSwapAnimator")]
	public class BlockedSwapAnimator : AAnimator<BlockedSwap>
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x000B63FC File Offset: 0x000B47FC
		public void OnEnable()
		{
			if (this.animationIds.IsNullOrEmptyCollection())
			{
				this.animationIds = new Dictionary<IntVector2, AnimationClip>
				{
					{
						IntVector2.Up,
						this.clipUp
					},
					{
						IntVector2.Right,
						this.clipRight
					},
					{
						IntVector2.Down,
						this.clipDown
					},
					{
						IntVector2.Left,
						this.clipLeft
					}
				};
			}
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000B646C File Offset: 0x000B486C
		protected override void DoAppend(BlockedSwap swap)
		{
			this.audioService.PlaySFX(AudioId.BlockedSwap, false, false, false);
			GemView gemView = base.GetGemView(swap.from, true);
			base.BlockSequence(this.sequence, gemView.gameObject, 0f);
			IntVector2 key = IntVector2.Direction(swap.from, swap.to);
			AnimationClip clip = this.animationIds[key];
			gemView.Play(clip, base.ModifiedDuration, true);
		}

		// Token: 0x040052AE RID: 21166
		[SerializeField]
		private AnimationClip clipUp;

		// Token: 0x040052AF RID: 21167
		[SerializeField]
		private AnimationClip clipDown;

		// Token: 0x040052B0 RID: 21168
		[SerializeField]
		private AnimationClip clipLeft;

		// Token: 0x040052B1 RID: 21169
		[SerializeField]
		private AnimationClip clipRight;

		// Token: 0x040052B2 RID: 21170
		private Dictionary<IntVector2, AnimationClip> animationIds;
	}
}
