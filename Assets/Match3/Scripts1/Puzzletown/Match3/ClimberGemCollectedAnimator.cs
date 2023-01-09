using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Audio;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200065E RID: 1630
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Climber/ClimberGemCollectedAnimator")]
	public class ClimberGemCollectedAnimator : AAnimator<ClimberGemCollected>
	{
		// Token: 0x0600290E RID: 10510 RVA: 0x000B7CC4 File Offset: 0x000B60C4
		protected override void DoAppend(ClimberGemCollected collected)
		{
			bool flag = this.boardView.currentTier > 0 || (float)this.boardView.currentLevel > this.levelToSpeedUp || (this.boardView.currentTier == 0 && this.boardView.currentLevel == 0);
			this.animationDuration = ((!flag) ? base.ModifiedDuration : this.zeroAnimDuration);
			float num = this.animController.fieldDelays[collected.Origin.position];
			float blockedDuration = this.coolDownDuration / this.animController.speed;
			GemView gemView = this.boardView.CreateTemporaryGemView(collected.Origin);
			GemView climber = base.GetGemView(collected.ClimberPos, true);
			Tween jumpTween = JumpAnimatorHelper.GetJumpTween(this.boardView, gemView.gameObject, collected.ClimberPos, this.animController, 2f, 9f);
			jumpTween.OnComplete(delegate
			{
				gemView.sprite.sortingLayerName = "Gems";
				gemView.Release(0f);
				climber.AttachFxToGemViewAsChild(this.boardView.objectPool.Get(this.climberChargeVfx));
				climber.GetComponentInChildren<ClimberView>().SwitchState(ClimberState.Move);
				this.audioService.PlaySFX(AudioId.ClimberGemChargesClimber, false, false, false);
			});
			float num2 = num + this.animationDuration;
			float atPosition = num2 + jumpTween.Duration(true);
			if (this.animationDuration > 0f)
			{
				this.sequence.Insert(num, this.BlockBoardView(this.animationDuration));
			}
			this.sequence.Insert(num2, jumpTween);
			this.sequence.Insert(atPosition, this.BlockBoardView(blockedDuration));
			this.boardView.StartCoroutine(this.PlaySoundAndFx(num, gemView));
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000B7E6C File Offset: 0x000B626C
		private IEnumerator PlaySoundAndFx(float delay, GemView collectedGem)
		{
			yield return new WaitForSeconds(delay);
			collectedGem.sprite.sortingLayerName = "FieldsOverlay";
			collectedGem.StartParticles(this.trailVfx, true);
			this.audioService.PlaySFX(AudioId.ClimberGemCollected, false, false, false);
			if (this.animationDuration > 0f)
			{
				collectedGem.Play(this.collectedAnimation, this.animationDuration, true);
				collectedGem.AttachFxToGemViewAsChild(this.boardView.objectPool.Get(this.matchedVfx));
			}
			yield break;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000B7E95 File Offset: 0x000B6295
		private DG.Tweening.Tween BlockBoardView(float blockedDuration)
		{
			return this.boardView.transform.DOLocalRotate(Vector3.zero, blockedDuration, RotateMode.Fast);
		}

		// Token: 0x040052C6 RID: 21190
		[SerializeField]
		private AnimationClip collectedAnimation;

		// Token: 0x040052C7 RID: 21191
		[SerializeField]
		private GameObject matchedVfx;

		// Token: 0x040052C8 RID: 21192
		[SerializeField]
		private GameObject trailVfx;

		// Token: 0x040052C9 RID: 21193
		[SerializeField]
		private GameObject climberChargeVfx;

		// Token: 0x040052CA RID: 21194
		[SerializeField]
		private float coolDownDuration;

		// Token: 0x040052CB RID: 21195
		[SerializeField]
		private float levelToSpeedUp;

		// Token: 0x040052CC RID: 21196
		private float zeroAnimDuration;

		// Token: 0x040052CD RID: 21197
		private float animationDuration;
	}
}
