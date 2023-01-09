using System.Collections;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000659 RID: 1625
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Chameleon/ChameleonMatchAnimator")]
	public class ChameleonMatchAnimator : AAnimator<ChameleonMatched>
	{
		// Token: 0x060028FE RID: 10494 RVA: 0x000B748C File Offset: 0x000B588C
		protected override void DoAppend(ChameleonMatched matchResult)
		{
			IntVector2 position = matchResult.OriginGem.position;
			FieldView fieldView = this.boardView.GetFieldView(position);
			float num = this.animController.fieldDelays[position];
			float delay = (!matchResult.CountForObjective) ? (num + this.colorChangeBlockingDelay) : num;
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(matchResult, num, fieldView));
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000B7510 File Offset: 0x000B5910
		private IEnumerator DelayedDoAppend(ChameleonMatched matchResult, float delay, FieldView view)
		{
			yield return new WaitForSeconds(delay);
			IntVector2 chameleonPosition = matchResult.OriginGem.position;
			if (matchResult.CountForObjective)
			{
				this.boardView.onModifierCollected.Dispatch(view.transform, "chameleon");
				FieldView fieldView = this.boardView.GetFieldView(chameleonPosition);
				base.SetFxToFieldview(this.boardView.objectPool.Get(this.chameleonCollectedFx), fieldView);
				this.audioService.PlaySFX(AudioId.ChameleonIsCollected, false, false, false);
			}
			else
			{
				ChameleonView chameleonView = this.boardView.GetChameleonView(chameleonPosition);
				chameleonView.SetChameleonIsStuck(false);
				GemColor nextColor = matchResult.NextColor;
				this.PlayColorChangeVfx(chameleonView, nextColor);
				this.audioService.PlaySFX(AudioId.ChameleonChangesColor, false, false, false);
				yield return new WaitForSeconds(this.colorChangeDelay);
				chameleonView.SetColor(nextColor, matchResult.ForeshadowingColor);
			}
			yield break;
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000B7540 File Offset: 0x000B5940
		private void PlayColorChangeVfx(ChameleonView chameleonView, GemColor nextColor)
		{
			GameObject gameObject = this.boardView.objectPool.Get(this.updateGemFx);
			Transform transform = chameleonView.transform;
			gameObject.transform.parent = transform;
			gameObject.transform.position = transform.position;
			gameObject.transform.rotation = Quaternion.identity;
			ParticleStartValues component = gameObject.GetComponent<ParticleStartValues>();
			component.UseColor = (int)nextColor;
		}

		// Token: 0x040052C0 RID: 21184
		public GameObject updateGemFx;

		// Token: 0x040052C1 RID: 21185
		public GameObject chameleonCollectedFx;

		// Token: 0x040052C2 RID: 21186
		public float colorChangeDelay = 0.2f;

		// Token: 0x040052C3 RID: 21187
		public float colorChangeBlockingDelay = 0.5f;
	}
}
