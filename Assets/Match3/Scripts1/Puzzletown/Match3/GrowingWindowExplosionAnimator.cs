using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200066D RID: 1645
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/GrowingWindowExplosionAnimator")]
	public class GrowingWindowExplosionAnimator : AAnimator<GrowingWindowExplosion>
	{
		// Token: 0x06002936 RID: 10550 RVA: 0x000B9070 File Offset: 0x000B7470
		protected override void DoAppend(GrowingWindowExplosion matchResult)
		{
			FieldView fieldView = this.boardView.GetFieldView(matchResult.Position);
			FieldView viewBelow = (!(matchResult.BelowPosition != Fields.invalidPos)) ? null : this.boardView.GetFieldView(matchResult.BelowPosition);
			float num = this.animController.fieldDelays[matchResult.CreatedFrom];
			if (matchResult.Position != matchResult.CreatedFrom)
			{
				num += this.cascadeDelay / this.animController.speed;
			}
			base.BlockSequence(this.sequence, fieldView.gameObject, num);
			this.boardView.StartCoroutine(this.DelayedDoAppend(num, fieldView, viewBelow));
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000B912C File Offset: 0x000B752C
		private IEnumerator DelayedDoAppend(float delay, FieldView view, FieldView viewBelow = null)
		{
			yield return new WaitForSeconds(delay);
			view.AnimateClearGrowingWindow();
			this.PlayClearEffect(view);
			if (viewBelow != null)
			{
				viewBelow.SetGrowingWindow(GrowingWindowView.GrowDirection.None);
			}
			this.audioService.PlaySFX(AudioId.GrowingWindowExplosion, false, false, false);
			yield return this.clearFinishedTime;
			view.ResetAndDisableGrowingWindow();
			yield break;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000B915C File Offset: 0x000B755C
		private void PlayClearEffect(FieldView view)
		{
			GameObject prefabInstance = this.boardView.objectPool.Get(this.leafsParticleFX.gameObject);
			base.SetFxToFieldview(prefabInstance, view);
			GameObject prefabInstance2 = this.boardView.objectPool.Get(this.dustParticleFX.gameObject);
			base.SetFxToFieldview(prefabInstance2, view);
		}

		// Token: 0x040052E0 RID: 21216
		[SerializeField]
		private float cascadeDelay = 0.0825f;

		// Token: 0x040052E1 RID: 21217
		[SerializeField]
		private ParticleSystem dustParticleFX;

		// Token: 0x040052E2 RID: 21218
		[SerializeField]
		private ParticleSystem leafsParticleFX;

		// Token: 0x040052E3 RID: 21219
		private readonly WaitForSeconds clearFinishedTime = new WaitForSeconds(0.4f);
	}
}
