using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200065B RID: 1627
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Chameleon/ChameleonSpawnAnimator")]
	public class ChameleonSpawnAnimator : AAnimator<ChameleonSpawn>
	{
		// Token: 0x06002907 RID: 10503 RVA: 0x000B7A28 File Offset: 0x000B5E28
		protected override void DoAppend(ChameleonSpawn chameleonSpawn)
		{
			SpawnResult spawn = chameleonSpawn.Spawn;
			FieldView fieldView = this.boardView.GetFieldView(chameleonSpawn.Position);
			GemView gemView = this.boardView.CreateGemView(spawn.gem, fieldView.transform);
			Vector3 b = (!(chameleonSpawn.FacingDirection == IntVector2.Up)) ? Vector3.up : Vector3.down;
			gemView.transform.position += b;
			Vector3 endValue = (Vector3)spawn.Position;
			this.sequence.Insert(0f, gemView.transform.DOMove(endValue, base.ModifiedDuration, false).SetEase(Ease.Linear));
			this.dropAnimator.PlayDropAnimation(gemView, spawn.IsFinal, base.ModifiedDuration, this.animController.landingDuration / this.animController.speed);
			this.audioService.PlaySFX(AudioId.ReplaceGem, false, false, false);
		}
	}
}
