using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.DataStructures;
using UnityEngine;

// Token: 0x02000683 RID: 1667
namespace Match3.Scripts1
{
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/StackedGemAnimator")]
	public class StackedGemAnimator : AAnimator<StackedGemExplosion>
	{
		// Token: 0x0600298C RID: 10636 RVA: 0x000BC440 File Offset: 0x000BA840
		protected override void DoAppend(StackedGemExplosion explosion)
		{
			float num = this.animController.fieldDelays[explosion.Center];
			Gem gem = explosion.gem;
			gem.type = GemType.Undefined;
			GemColor color = gem.color;
			int count = explosion.Group.Count;
			for (int i = 0; i < count; i++)
			{
				Gem target = explosion.Group[i];
				target.color = color;
				this.colors[target.position] = color;
				this.MoveGemToTarget(gem, target, num);
			}
			this.boardView.StartCoroutine(this.DelayedDoAppend(num));
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000BC4E8 File Offset: 0x000BA8E8
		private IEnumerator DelayedDoAppend(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.audioService.PlaySFX(AudioId.StackedGemActivated, false, false, false);
			yield break;
		}

		// eli key point 堆叠宝石动画
		private void MoveGemToTarget(Gem origin, Gem target, float originDelay)
		{
			GemView gemView = this.boardView.CreateTemporaryGemView(origin);
			gemView.sprite.sortingLayerName = "FieldsOverlay";
			// gemView.sprite.sortingLayerName = "Gems";	// 不清楚原来为什么是FieldsOverlay, 就应该是FieldOverlay
			gemView.StartParticles();
			Tweener t = gemView.transform.DOScale(Vector3.one, 0f);
			// Debug.LogError("targetPos:" + target.position);
			Tween jumpTween = JumpAnimatorHelper.GetJumpTween(this.boardView, gemView.gameObject, target.position, this.animController, 2f, 9f);
			gemView.transform.localScale = Vector3.zero;
			float num = jumpTween.Duration(true);
			float num2 = num + originDelay;
			float num3 = this.animController.fieldDelays[target.position];
			if (num3 < num2)
			{
				Map<float> fieldDelays;
				IntVector2 position;
				(fieldDelays = this.animController.fieldDelays)[position = target.position] = fieldDelays[position] + (num2 - num3);
			}
			float num4 = this.animController.fieldDelays[target.position];
			float atPosition = num4 - num;
			this.sequence.Insert(atPosition, t);
			jumpTween.OnComplete(delegate
			{
				// Debug.LogError("<color=blue>tween动画完成</color>, 然后从boardview中添加或移除？");
				GemView oldGemView = this.boardView.GetGemView(target.position, false);
				target.color = this.colors[target.position];
				if (oldGemView == null)
				{
					// Debug.LogError("新建");
					this.boardView.CreateGemView(target);
				}
				else
				{
					// Debug.LogError("替换");
					oldGemView.Show(target);
				}
				oldGemView.sprite.sortingLayerName = "Gems";
				// oldGemView.Release(0f);	// 大概可能是因为变量重名，导致
				gemView.Release(0f);
				this.audioService.PlaySFX(AudioId.ReplaceGem, false, false, false);
				// Debug.LogError("tween动画完成2");
			});
			this.sequence.Insert(atPosition, jumpTween);
		}

		// Token: 0x0400531C RID: 21276
		private Map<GemColor> colors = new Map<GemColor>(9);
	}
}
