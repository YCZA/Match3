using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200066E RID: 1646
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Fields/GrowingWindowSpawnAnimator")]
	public class GrowingWindowSpawnAnimator : AAnimator<GrowingWindowSpawn>
	{
		// Token: 0x0600293A RID: 10554 RVA: 0x000B92EC File Offset: 0x000B76EC
		protected override void DoAppend(GrowingWindowSpawn matchResult)
		{
			if (!matchResult.DestroyedGem.Equals(default(Gem)))
			{
				GemView gemView = this.boardView.GetGemView(matchResult.Target, true);
				this.boardView.ReleaseView(gemView, this.matchAnimator.duration);
			}
			FieldView fieldView = this.boardView.GetFieldView(matchResult.Target);
			bool flag = matchResult.Above != Fields.invalidPos;
			bool flag2 = matchResult.Below != Fields.invalidPos;
			bool flag3 = matchResult.Left != Fields.invalidPos;
			bool flag4 = matchResult.Right != Fields.invalidPos;
			GrowingWindowView.GrowDirection growDirection = GrowingWindowView.GrowDirection.None;
			if (flag2)
			{
				growDirection = GrowingWindowView.GrowDirection.Up;
				if (flag)
				{
					GrowingWindowView.GrowDirection growDirection2 = (GrowingWindowView.GrowDirection)RandomHelper.Next(1, 3);
					growDirection = growDirection2;
					if (growDirection == GrowingWindowView.GrowDirection.Up)
					{
						this.boardView.StartCoroutine(this.DelayedSetGrowingWindowWithBridge(fieldView));
					}
				}
				FieldView fieldView2 = this.boardView.GetFieldView(matchResult.Below);
				this.boardView.StartCoroutine(this.DelayedSetGrowingWindowWithBridge(fieldView2));
			}
			else if (flag)
			{
				growDirection = GrowingWindowView.GrowDirection.Down;
			}
			else if (flag3)
			{
				growDirection = GrowingWindowView.GrowDirection.Left;
			}
			else if (flag4)
			{
				growDirection = GrowingWindowView.GrowDirection.Right;
			}
			fieldView.SetGrowingWindow(growDirection);
			FieldView fieldView3 = this.boardView.GetFieldView(matchResult.Target);
			this.boardView.StartCoroutine(this.PlayExplosionEffect(fieldView3));
			this.audioService.PlaySFX(AudioId.GrowingWindowSpawn, false, false, false);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000B9478 File Offset: 0x000B7878
		private IEnumerator PlayExplosionEffect(FieldView view)
		{
			yield return new WaitForSeconds(this.dustDelay);
			GameObject dustParticleSystem = this.boardView.objectPool.Get(this.dustParticleFX.gameObject);
			base.SetFxToFieldview(dustParticleSystem, view);
			yield break;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000B949C File Offset: 0x000B789C
		private IEnumerator DelayedSetGrowingWindowWithBridge(FieldView view)
		{
			yield return this.spawnFinishedTime;
			view.SetGrowingWindowWithBridge();
			yield break;
		}

		// Token: 0x040052E4 RID: 21220
		[SerializeField]
		private float dustDelay = 0.2f;

		// Token: 0x040052E5 RID: 21221
		[SerializeField]
		private ParticleSystem dustParticleFX;

		// Token: 0x040052E6 RID: 21222
		private readonly WaitForSeconds spawnFinishedTime = new WaitForSeconds(0.55f);
	}
}
