using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000660 RID: 1632
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Climber/ClimberSpawnAnimator")]
	public class ClimberSpawnAnimator : AAnimator<ClimberSpawn>
	{
		// Token: 0x06002914 RID: 10516 RVA: 0x000B80B4 File Offset: 0x000B64B4
		protected override void DoAppend(ClimberSpawn climberSpawn)
		{
			SpawnResult spawn = climberSpawn.spawn;
			FieldView fieldView = this.boardView.GetFieldView(climberSpawn.Position);
			GemView gemView = this.boardView.CreateGemView(spawn.gem, fieldView.transform);
			gemView.transform.position += Vector3.down;
			if (climberSpawn.fromPortal)
			{
				gemView.GetComponentInChildren<ClimberView>().SwitchState(ClimberState.Move);
			}
			Vector3 endValue = (Vector3)spawn.Position;
			this.sequence.Insert(0f, gemView.transform.DOMove(endValue, base.ModifiedDuration, false).SetEase(Ease.Linear));
			this.dropAnimator.PlayDropAnimation(gemView, spawn.IsFinal, base.ModifiedDuration, this.animController.landingDuration / this.animController.speed);
			this.audioService.PlaySFX(AudioId.ReplaceGem, false, false, false);
		}
	}
}
