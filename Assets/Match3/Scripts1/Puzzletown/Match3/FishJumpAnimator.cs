using DG.Tweening;
using Match3.Scripts1.Shared.DataStructures;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200066B RID: 1643
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/FishJumpAnimator")]
	public class FishJumpAnimator : AAnimator<JumpResult>
	{
		// Token: 0x06002932 RID: 10546 RVA: 0x000B8EB0 File Offset: 0x000B72B0
		protected override void DoAppend(JumpResult jump)
		{
			if (jump.target != Fields.invalidPos)
			{
				GameObject gameObject = this.boardView.objectPool.Get(this.objectPrefab);
				Vector3 position = (Vector3)jump.origin;
				position.x += BoardView.fieldOffset;
				position.y += BoardView.fieldOffset;
				gameObject.transform.position = position;
				Tween jumpTween = JumpAnimatorHelper.GetJumpTween(this.boardView, gameObject, jump.target, this.animController, this.jumpForce, this.fishSpeed);
				float num = jumpTween.Duration(true);
				ButterflyController component = gameObject.GetComponent<ButterflyController>();
				component.Initialize(jump.target, num, jump.color, this.boardView.objectPool);
				float createEffectDuration = component.GetCreateEffectDuration();
				component.StartButterfly();
				Map<float> fieldDelays;
				IntVector2 target;
				(fieldDelays = this.animController.fieldDelays)[target = jump.target] = fieldDelays[target] + (createEffectDuration + num);
				this.sequence.Insert(createEffectDuration, jumpTween);
			}
		}

		// Token: 0x040052DD RID: 21213
		public float fishSpeed = 9f;

		// Token: 0x040052DE RID: 21214
		public float jumpForce = 2f;

		// Token: 0x040052DF RID: 21215
		public GameObject objectPrefab;
	}
}
