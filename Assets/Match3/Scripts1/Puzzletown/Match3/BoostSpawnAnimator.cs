using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000654 RID: 1620
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/BoostSpawnAnimator")]
	public class BoostSpawnAnimator : AAnimator<BoostSpawnResult>
	{
		// Token: 0x060028ED RID: 10477 RVA: 0x000B6920 File Offset: 0x000B4D20
		protected override void DoAppend(BoostSpawnResult spawn)
		{
			GemView gemView = this.boardView.GetGemView(spawn.Position, true);
			this.sequence.InsertCallback(base.ModifiedDuration, delegate
			{
				gemView.Show(spawn.gem);
			});
			this.dropAnimator.PlayDropAnimation(gemView, spawn.IsFinal, base.ModifiedDuration, this.animController.landingDuration / this.animController.speed);
			this.audioService.PlaySFX(AudioId.ReplaceGem, false, false, false);
		}
	}
}
