using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000682 RID: 1666
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/SpawnAnimator")]
	public class SpawnAnimator : AAnimator<SpawnResult>
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x000BC380 File Offset: 0x000BA780
		protected override void DoAppend(SpawnResult spawn)
		{
			GemView gemView = this.SpawnGemView(spawn);
			FieldView fieldView = this.boardView.GetFieldView(spawn.position);
			this.sequence.Insert(0f, gemView.transform.DOMove(fieldView.transform.position, base.ModifiedDuration, false).SetEase(Ease.Linear));
			this.dropAnimator.PlayDropAnimation(gemView, spawn.IsFinal, base.ModifiedDuration, this.animController.landingDuration / this.animController.speed);
			this.audioService.PlaySFX(AudioId.Trickle, false, false, false);
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000BC41D File Offset: 0x000BA81D
		private GemView SpawnGemView(SpawnResult spawnResult)
		{
			return this.boardView.CreateGemView(spawnResult);
		}
	}
}
