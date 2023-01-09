using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000666 RID: 1638
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/DefinedSpawnNeededAnimator")]
	public class DefinedSpawnNeededAnimator : AAnimator<DefinedSpawnNeeded>
	{
		// Token: 0x06002926 RID: 10534 RVA: 0x000B89EC File Offset: 0x000B6DEC
		protected override void DoAppend(DefinedSpawnNeeded spawnNeeded)
		{
			FieldView fieldView = this.boardView.GetFieldView(spawnNeeded.Position);
			float num = this.animController.fieldDelays[spawnNeeded.Position];
			this.boardView.StartCoroutine(this.DelayedAppend(num + this.delayToClose, fieldView));
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000B8A40 File Offset: 0x000B6E40
		private IEnumerator DelayedAppend(float delay, FieldView fieldView)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.DefinedGemExploded, false, false, false);
			fieldView.PlayDefinedSpawningAnimation(true);
			yield break;
		}

		// Token: 0x040052D8 RID: 21208
		public float delayToClose = 0.2f;
	}
}
