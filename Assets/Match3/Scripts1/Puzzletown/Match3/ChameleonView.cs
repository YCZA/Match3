using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000698 RID: 1688
	public class ChameleonView : AAnimatedGemOverlayView
	{
		// Token: 0x06002A23 RID: 10787 RVA: 0x000C1136 File Offset: 0x000BF536
		private void Start()
		{
			this.SetColor(this.color, this.foreshadowingColor);
			if (this.chameleonAnimator.GetBool(ChameleonView.IS_STUCK))
			{
				this.RandomizeAnimationStart();
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000C1168 File Offset: 0x000BF568
		public void Initialize(Gem gem, bool isFieldChained = false)
		{
			GemColor gemColor = gem.color;
			ChameleonVariant key = (!gem.IsAllColorChameleon) ? ChameleonVariant.Reduced : ChameleonVariant.All;
			GemColor gemColor2 = GemColor.Undefined;
			if (!Fields.chameleonModels.IsNullOrEmptyCollection())
			{
				gemColor2 = Fields.chameleonModels[key].GetNextColor(gemColor);
			}
			GemDirection direction = gem.direction;
			this.SetFacingDirection(direction);
			bool chameleonIsStuck = gem.IsIced || isFieldChained;
			this.SetChameleonIsStuck(chameleonIsStuck);
			this.SetColor(gemColor, gemColor2);
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000C11E8 File Offset: 0x000BF5E8
		public void SetColor(GemColor gemColor, GemColor foreshadowingColor)
		{
			this.color = gemColor;
			this.foreshadowingColor = foreshadowingColor;
			float valueForLookUpTableColumn = FieldView.GetValueForLookUpTableColumn(gemColor);
			this.meshRenderer.material.SetFloat(ChameleonView.LUT_COLOR, valueForLookUpTableColumn);
			if (foreshadowingColor != GemColor.Undefined)
			{
				valueForLookUpTableColumn = FieldView.GetValueForLookUpTableColumn(foreshadowingColor);
				this.meshRenderer.material.SetFloat(ChameleonView.LUT_NEXT_COLOR, valueForLookUpTableColumn);
			}
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000C1244 File Offset: 0x000BF644
		public void SetFacingDirection(GemDirection facingDirection)
		{
			base.transform.localScale = this.scaleRight;
			int num = 0;
			if (facingDirection == GemDirection.Up)
			{
				num = -90;
			}
			else if (facingDirection == GemDirection.Right)
			{
				base.transform.localScale = this.scaleLeft;
				num = -180;
			}
			else if (facingDirection == GemDirection.Down)
			{
				num = -270;
			}
			base.transform.rotation = Quaternion.Euler(0f, 0f, (float)num);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000C12BF File Offset: 0x000BF6BF
		public void SetChameleonIsStuck(bool isStuck)
		{
			this.chameleonAnimator.SetBool(ChameleonView.IS_STUCK, isStuck);
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000C12D2 File Offset: 0x000BF6D2
		public void SetDebugTypeText(ChameleonVariant variant)
		{
			this.debugText.gameObject.SetActive(true);
			this.debugText.text = ((variant != ChameleonVariant.All) ? "RE" : "ALL");
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000C1307 File Offset: 0x000BF707
		public void PlayMoveAnimation()
		{
			this.chameleonAnimator.SetTrigger(ChameleonView.MOVE);
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000C131C File Offset: 0x000BF71C
		private void RandomizeAnimationStart()
		{
			float deltaTime = (float)RandomHelper.Next(50) / 10f;
			this.chameleonAnimator.Update(deltaTime);
		}

		// Token: 0x04005389 RID: 21385
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x0400538A RID: 21386
		[SerializeField]
		private TextMesh debugText;

		// Token: 0x0400538B RID: 21387
		[SerializeField]
		private Animator chameleonAnimator;

		// Token: 0x0400538C RID: 21388
		private static readonly int LUT_COLOR = Shader.PropertyToID("_ColorValue");

		// Token: 0x0400538D RID: 21389
		private static readonly int LUT_NEXT_COLOR = Shader.PropertyToID("_NextColorValue");

		// Token: 0x0400538E RID: 21390
		private static readonly int MOVE = Animator.StringToHash("Move");

		// Token: 0x0400538F RID: 21391
		private static readonly int IS_STUCK = Animator.StringToHash("IsStuck");

		// Token: 0x04005390 RID: 21392
		private readonly Vector3 scaleRight = new Vector3(-1f, 1f, 1f);

		// Token: 0x04005391 RID: 21393
		private readonly Vector3 scaleLeft = new Vector3(-1f, -1f, 1f);

		// Token: 0x04005392 RID: 21394
		private GemColor color;

		// Token: 0x04005393 RID: 21395
		private GemColor foreshadowingColor;
	}
}
