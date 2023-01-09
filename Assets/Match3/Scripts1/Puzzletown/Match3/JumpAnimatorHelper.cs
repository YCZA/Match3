using DG.Tweening;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000676 RID: 1654
	public static class JumpAnimatorHelper
	{
		// Token: 0x0600294E RID: 10574 RVA: 0x000B9E04 File Offset: 0x000B8204
		public static DG.Tweening.Tween GetJumpTween(BoardView boardView, GameObject go, IntVector2 targetGridPos, BoardAnimationController animController, float jumpForce = DEFAULT_FORCE, float speed = DEFAULT_SPEED)
		{
			go.transform.SetParent(boardView.transform);
			Vector3 vector = (Vector3)targetGridPos;
			float jumpDuration = JumpAnimatorHelper.GetJumpDuration(go.transform.position, vector, animController, jumpForce, speed);
			// eli key point do local jump
			// 动画时长不能为0，不然不执行完成事件!!!!
			return go.transform.DOLocalJump(vector, jumpForce, 1, jumpDuration, false).SetEase(Ease.Linear);
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000B9E5C File Offset: 0x000B825C
		private static float GetJumpDuration(Vector3 origin, Vector3 target, BoardAnimationController animController, float jumpForce, float fishSpeed)
		{
			Vector3 vector = target - origin;
			Vector3 vector2 = vector * 0.5f;
			Vector3 a = new Vector3(vector2.x, vector2.y + jumpForce, vector2.z);
			float num = Mathf.Sqrt(a.x * a.x + a.y * a.y);
			Vector3 vector3 = a - vector;
			float num2 = Mathf.Sqrt(vector3.x * vector3.x + vector3.y * vector3.y);
			float num3 = num + num2;
			return num3 / fishSpeed / animController.speed;
		}

		// Token: 0x040052F4 RID: 21236
		private const float DEFAULT_SPEED = 9f;

		// Token: 0x040052F5 RID: 21237
		private const float DEFAULT_FORCE = 2f;
	}
}
