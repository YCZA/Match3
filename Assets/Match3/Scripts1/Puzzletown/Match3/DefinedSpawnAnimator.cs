using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000665 RID: 1637
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/DefinedSpawnAnimator")]
	public class DefinedSpawnAnimator : AAnimator<DefinedSpawnResult>
	{
		// Token: 0x06002923 RID: 10531 RVA: 0x000B8924 File Offset: 0x000B6D24
		protected override void DoAppend(DefinedSpawnResult spawn)
		{
			FieldView fieldView = this.boardView.GetFieldView(spawn.position);
			GemView gemView = this.SpawnGemView(spawn.gem, fieldView.transform);
			gemView.transform.localScale = Vector3.zero;
			this.sequence.Insert(0f, gemView.transform.DOScale(Vector3.one, base.ModifiedDuration).SetEase(Ease.Linear));
			fieldView.PlayDefinedSpawningAnimation(false);
			base.BlockSequence(this.sequence, fieldView.gameObject, 0f);
			this.audioService.PlaySFX(AudioId.DefinedGemSpawned, false, false, false);
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000B89C7 File Offset: 0x000B6DC7
		private GemView SpawnGemView(Gem spawningGem, Transform fieldView)
		{
			return this.boardView.CreateGemView(spawningGem, fieldView);
		}
	}
}
