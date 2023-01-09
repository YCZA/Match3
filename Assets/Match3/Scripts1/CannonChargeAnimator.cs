using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.DataStructures;
using UnityEngine;

// Token: 0x02000656 RID: 1622
namespace Match3.Scripts1
{
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/CannonChargeAnimator")]
	public class CannonChargeAnimator : AAnimator<CannonCharged>
	{
		// Token: 0x060028F2 RID: 10482 RVA: 0x000B6BE8 File Offset: 0x000B4FE8
		protected override void DoAppend(CannonCharged cannonCharged)
		{
			float num = this.animController.fieldDelays[cannonCharged.chargingGem.position];
			Gem chargingGem = cannonCharged.chargingGem;
			chargingGem.type = GemType.Undefined;
			this.MoveGemToTarget(chargingGem, cannonCharged.chargedCannonPosition, cannonCharged.chargeAmount, num);
			this.boardView.StartCoroutine(this.DelayedDoAppend(num));
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000B6C50 File Offset: 0x000B5050
		private IEnumerator DelayedDoAppend(float delay)
		{
			yield return new WaitForSeconds(delay);
			yield break;
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000B6C6C File Offset: 0x000B506C
		private void MoveGemToTarget(Gem origin, IntVector2 cannonPosition, int chargeAmount, float originDelay)
		{
			GemView gemView = this.boardView.CreateTemporaryGemView(origin);
			gemView.sprite.sortingLayerName = "FieldsOverlay";
			gemView.StartParticles();
			Tweener t = gemView.transform.DOScale(Vector3.one, 0f);
			Tween jumpTween = JumpAnimatorHelper.GetJumpTween(this.boardView, gemView.gameObject, cannonPosition, this.animController, 2f, 9f);
			gemView.transform.localScale = Vector3.zero;
			float num = jumpTween.Duration(true);
			float num2 = num + originDelay;
			float num3 = this.animController.fieldDelays[cannonPosition];
			if (num3 < num2)
			{
				Map<float> fieldDelays;
				IntVector2 cannonPosition2;
				(fieldDelays = this.animController.fieldDelays)[cannonPosition2 = cannonPosition] = fieldDelays[cannonPosition2] + (num2 - num3);
			}
			float num4 = this.animController.fieldDelays[cannonPosition];
			float atPosition = num4 - num;
			this.sequence.Insert(atPosition, t);
			jumpTween.OnComplete(delegate
			{
				gemView.sprite.sortingLayerName = "Gems";
				gemView.Release(0f);
				this.UpdateCannonView(cannonPosition, chargeAmount);
			});
			this.sequence.Insert(atPosition, jumpTween);
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000B6DCC File Offset: 0x000B51CC
		private void UpdateCannonView(IntVector2 cannonPosition, int chargeAmount)
		{
			Gem gem = this.boardView.GetField(cannonPosition).gem;
			GemView gemView = this.boardView.GetGemView(cannonPosition, true);
			if (gemView.gemParameterShown != gem.parameter)
			{
				gemView.UpdateModifierView<CannonView>(gem, chargeAmount);
			}
			this.audioService.PlaySFX(AudioId.CannonCharge, false, false, false);
		}
	}
}
